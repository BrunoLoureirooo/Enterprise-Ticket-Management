using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using teams.Entities.Models;
using teams.Repository.Contracts;

namespace teams.Application.Messaging
{
    public class UserSyncConsumer(
        IConfiguration config,
        IServiceScopeFactory scopeFactory,
        ILogger<UserSyncConsumer> logger) : BackgroundService
    {
        private const string Exchange  = "identity.users";
        private const string QueueName = "teams.identity.users";

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => Task.Run(() => RunConsumer(stoppingToken), stoppingToken);

        private void RunConsumer(CancellationToken stoppingToken)
        {
            var uri = config["RabbitMQ:ConnectionString"] ?? "amqp://guest:guest@localhost/";

            IConnection? connection = null;
            IModel? channel = null;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var factory = new ConnectionFactory { Uri = new Uri(uri) };
                    connection = factory.CreateConnection();
                    channel    = connection.CreateModel();

                    channel.ExchangeDeclare(Exchange, ExchangeType.Fanout, durable: true);
                    channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false);
                    channel.QueueBind(QueueName, Exchange, routingKey: string.Empty);
                    channel.BasicQos(0, 1, false);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (_, ea) => HandleMessage(channel, ea);
                    channel.BasicConsume(QueueName, autoAck: false, consumer);

                    logger.LogInformation("UserSyncConsumer connected");
                    stoppingToken.WaitHandle.WaitOne();
                    break;
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning(ex, "UserSyncConsumer RabbitMQ connection failed, retrying in 5s");
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
                var evt = JsonSerializer.Deserialize<UserChangedEvent>(Encoding.UTF8.GetString(ea.Body.Span));
                if (evt is null) { channel.BasicNack(ea.DeliveryTag, false, false); return; }

                using var scope = scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
                ProcessEvent(repo, evt).GetAwaiter().GetResult();
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process UserChangedEvent");
                channel.BasicNack(ea.DeliveryTag, false, requeue: false);
            }
        }

        private static async Task ProcessEvent(IRepositoryManager repo, UserChangedEvent evt)
        {
            var existing = await repo.SyncedUser.GetAsync(evt.UserId);

            switch (evt.Action)
            {
                case "Created" when existing is null:
                    repo.SyncedUser.Add(new SyncedUser { UserId = evt.UserId, Name = evt.Name, Email = evt.Email });
                    break;
                case "Updated" when existing is not null:
                    existing.Name  = evt.Name;
                    existing.Email = evt.Email;
                    repo.SyncedUser.Update(existing);
                    break;
                case "Deleted" when existing is not null:
                    repo.SyncedUser.Remove(existing);
                    break;
            }

            await repo.SaveAsync();
        }

        // Mirror of identity-api's event
        private class UserChangedEvent
        {
            public Guid UserId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Action { get; set; } = string.Empty;
        }
    }
}
