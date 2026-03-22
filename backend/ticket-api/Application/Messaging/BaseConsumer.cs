using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ticket.Repository.Contracts;

namespace ticket.Application.Messaging
{
    public abstract class BaseConsumer(
        string exchange,
        string queueName,
        IConfiguration config,
        IServiceScopeFactory scopeFactory,
        ILogger logger) : BackgroundService
    {
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

                    channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: true);
                    channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
                    channel.QueueBind(queueName, exchange, routingKey: string.Empty);
                    channel.BasicQos(0, 1, false);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (_, ea) => HandleMessage(channel, ea);
                    channel.BasicConsume(queueName, autoAck: false, consumer);

                    logger.LogInformation("{Consumer} connected to {Exchange}", GetType().Name, exchange);
                    stoppingToken.WaitHandle.WaitOne();
                    break;
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning(ex, "{Consumer} connection failed, retrying in 5s", GetType().Name);
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
                using var scope = scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
                Handle(json, repo).GetAwaiter().GetResult();
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Consumer} failed to process message", GetType().Name);
                channel.BasicNack(ea.DeliveryTag, false, requeue: false);
            }
        }

        protected abstract Task Handle(string json, IRepositoryManager repo);

        protected static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json);
    }
}
