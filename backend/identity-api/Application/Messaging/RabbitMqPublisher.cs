using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace backend.Application.Messaging
{
    public class RabbitMqPublisher : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqPublisher> _logger;
        private const string UserExchange = "identity.users";

        public RabbitMqPublisher(IConfiguration config, ILogger<RabbitMqPublisher> logger)
        {
            _logger = logger;
            var uri = config["RabbitMQ:ConnectionString"] ?? "amqp://guest:guest@localhost/";
            var factory = new ConnectionFactory { Uri = new Uri(uri) };
            _connection = factory.CreateConnection();
            _channel    = _connection.CreateModel();
            _channel.ExchangeDeclare(UserExchange, ExchangeType.Fanout, durable: true);
        }

        public void PublishUser(UserChangedEvent evt)
        {
            try
            {
                var body  = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));
                var props = _channel.CreateBasicProperties();
                props.Persistent = true;
                _channel.BasicPublish(UserExchange, routingKey: string.Empty, props, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish UserChangedEvent for user {UserId}", evt.UserId);
            }
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
