using System.Text;
using System.Text.Json;
using backend.Entities.Models;
using backend.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace backend.Application.Messaging
{
    public class TeamMembershipConsumer : BackgroundService
    {
        private const string Exchange = "team.membership";
        private const string Queue    = "identity.team.membership";

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TeamMembershipConsumer> _logger;
        private readonly string _uri;

        public TeamMembershipConsumer(IServiceScopeFactory scopeFactory, IConfiguration config, ILogger<TeamMembershipConsumer> logger)
        {
            _scopeFactory = scopeFactory;
            _logger       = logger;
            _uri          = config["RabbitMQ:ConnectionString"] ?? "amqp://guest:guest@localhost/";
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = Task.Run(() => RunConsumer(stoppingToken), stoppingToken);
            return Task.CompletedTask;
        }

        private void RunConsumer(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var factory    = new ConnectionFactory { Uri = new Uri(_uri), DispatchConsumersAsync = true };
                    using var conn = factory.CreateConnection();
                    using var ch   = conn.CreateModel();

                    ch.ExchangeDeclare(Exchange, ExchangeType.Fanout, durable: true);
                    ch.QueueDeclare(Queue, durable: true, exclusive: false, autoDelete: false);
                    ch.QueueBind(Queue, Exchange, routingKey: string.Empty);
                    ch.BasicQos(0, 1, false);

                    var consumer = new AsyncEventingBasicConsumer(ch);
                    consumer.Received += async (_, ea) =>
                    {
                        try
                        {
                            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                            var evt  = JsonSerializer.Deserialize<MembershipChangedEvent>(json,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                            if (evt is not null)
                                await HandleAsync(evt);

                            ch.BasicAck(ea.DeliveryTag, false);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing team membership event");
                            ch.BasicNack(ea.DeliveryTag, false, requeue: true);
                        }
                    };

                    ch.BasicConsume(Queue, autoAck: false, consumer);
                    ct.WaitHandle.WaitOne();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "TeamMembershipConsumer connection failed, retrying in 5s");
                    Thread.Sleep(5000);
                }
            }
        }

        private async Task HandleAsync(MembershipChangedEvent evt)
        {
            using var scope   = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();

            var existing = await context.SyncedTeamMemberships
                .FirstOrDefaultAsync(m => m.UserId == evt.UserId && m.TeamId == evt.TeamId);

            if (evt.Action == "Removed")
            {
                if (existing is not null)
                {
                    context.SyncedTeamMemberships.Remove(existing);
                    await context.SaveChangesAsync();
                }
                return;
            }

            if (existing is null)
            {
                context.SyncedTeamMemberships.Add(new SyncedTeamMembership
                {
                    UserId   = evt.UserId,
                    TeamId   = evt.TeamId,
                    IsLeader = evt.IsLeader,
                });
            }
            else
            {
                existing.IsLeader = evt.IsLeader;
            }

            await context.SaveChangesAsync();
        }
    }
}
