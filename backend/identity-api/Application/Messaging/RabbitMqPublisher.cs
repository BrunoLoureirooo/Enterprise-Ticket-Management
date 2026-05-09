using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace backend.Application.Messaging
{
    public class RabbitMqPublisher : IDisposable
    {
        private readonly string _uri;
        private readonly ILogger<RabbitMqPublisher> _logger;
        private const string UserExchange = "identity.users";

        private IConnection? _connection;
        private IModel? _channel;
        private readonly object _lock = new();

        public RabbitMqPublisher(IConfiguration config, ILogger<RabbitMqPublisher> logger)
        {
            _logger = logger;
            _uri    = config["RabbitMQ:ConnectionString"] ?? "amqp://guest:guest@localhost/";
        }

        private IModel? GetChannel()
        {
            if (_channel is { IsOpen: true }) return _channel;
            lock (_lock)
            {
                if (_channel is { IsOpen: true }) return _channel;
                try
                {
                    _connection?.Dispose();
                    _channel?.Dispose();
                    var factory = new ConnectionFactory { Uri = new Uri(_uri) };
                    _connection = factory.CreateConnection();
                    _channel    = _connection.CreateModel();
                    _channel.ExchangeDeclare(UserExchange, ExchangeType.Fanout, durable: true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "RabbitMQ connection failed — user events will not be published");
                    _channel = null;
                }
            }
            return _channel;
        }

        public void PublishUser(UserChangedEvent evt)
        {
            try
            {
                var ch = GetChannel();
                if (ch is null) return;
                var body  = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));
                var props = ch.CreateBasicProperties();
                props.Persistent = true;
                ch.BasicPublish(UserExchange, routingKey: string.Empty, props, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish UserChangedEvent for user {UserId}", evt.UserId);
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
