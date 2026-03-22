using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ticket.Entities.Models;
using ticket.Repository.Contracts;

namespace ticket.Application.Messaging
{
    public class TeamMembershipConsumer(
        IConfiguration config,
        IServiceScopeFactory scopeFactory,
        ILogger<TeamMembershipConsumer> logger) : BackgroundService
    {
        private const string Exchange  = "team.membership";
        private const string QueueName = "ticket.team.membership";

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Connect on a background thread so startup isn't blocked if RabbitMQ is slow
            return Task.Run(() => RunConsumer(stoppingToken), stoppingToken);
        }

        private void RunConsumer(CancellationToken stoppingToken)
        {
            var uri = config["RabbitMQ:ConnectionString"] ?? "amqp://guest:guest@localhost/";

            IConnection? connection = null;
            IModel? channel = null;

            // Retry loop — RabbitMQ might not be ready immediately on startup
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var factory = new ConnectionFactory { Uri = new Uri(uri) };
                    connection = factory.CreateConnection();
                    channel    = connection.CreateModel();

                    // Declare the same exchange the teams service publishes to.
                    // "durable: true" means the exchange survives a RabbitMQ restart.
                    channel.ExchangeDeclare(Exchange, ExchangeType.Fanout, durable: true);

                    // Each consuming service gets its own named queue bound to the exchange.
                    // "durable: true" means the queue and any unprocessed messages survive restarts.
                    channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false);
                    channel.QueueBind(QueueName, Exchange, routingKey: string.Empty);

                    // Only fetch one message at a time — don't overwhelm the consumer
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (_, ea) => HandleMessage(channel, ea);

                    // autoAck: false — we manually ack after successful processing
                    channel.BasicConsume(QueueName, autoAck: false, consumer);

                    logger.LogInformation("TeamMembershipConsumer connected and listening");

                    // Block here until cancellation or connection drops
                    stoppingToken.WaitHandle.WaitOne();
                    break;
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning(ex, "RabbitMQ connection failed, retrying in 5s");
                    Thread.Sleep(5000);
                }
                finally
                {
                    channel?.Dispose();
                    connection?.Dispose();
                }
            }
        }

        private void HandleMessage(IModel channel, BasicDeliverEventArgs ea)
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.Span);
                var evt  = JsonSerializer.Deserialize<MembershipChangedEvent>(json);

                if (evt is null)
                {
                    channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                    return;
                }

                // Create a fresh DI scope for each message so we get a clean DbContext
                using var scope = scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();

                ProcessEvent(repo, evt).GetAwaiter().GetResult();

                // Tell RabbitMQ the message was processed successfully — remove from queue
                channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process membership event");
                // Nack without requeue — broken messages shouldn't loop forever
                channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
            }
        }

        private static async Task ProcessEvent(IRepositoryManager repo, MembershipChangedEvent evt)
        {
            var existing = await repo.TeamMembership.GetAsync(evt.UserId, evt.TeamId);

            switch (evt.Action)
            {
                case "Added" when existing is null:
                    repo.TeamMembership.Add(new TeamMembership
                    {
                        UserId   = evt.UserId,
                        TeamId   = evt.TeamId,
                        IsLeader = evt.IsLeader,
                    });
                    break;

                case "Updated" when existing is not null:
                    existing.IsLeader = evt.IsLeader;
                    repo.TeamMembership.Update(existing);
                    break;

                case "Removed" when existing is not null:
                    repo.TeamMembership.Remove(existing);
                    break;
            }

            await repo.SaveAsync();
        }
    }

    // Mirror of the event published by the teams service
    internal class MembershipChangedEvent
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
        public bool IsLeader { get; set; }
        public string Action { get; set; } = string.Empty;
    }
}
