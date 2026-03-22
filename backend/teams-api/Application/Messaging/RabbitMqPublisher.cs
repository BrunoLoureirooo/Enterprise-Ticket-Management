using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using teams.Entities.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace teams.Application.Messaging
{
    public class RabbitMqPublisher : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqPublisher> _logger;

        private const string MembershipExchange = "team.membership";
        private const string TeamsExchange      = "teams.teams";
        private const string ProjectsExchange   = "teams.projects";

        public RabbitMqPublisher(IConfiguration config, ILogger<RabbitMqPublisher> logger)
        {
            _logger = logger;
            var uri = config["RabbitMQ:ConnectionString"] ?? "amqp://guest:guest@localhost/";
            var factory = new ConnectionFactory { Uri = new Uri(uri) };
            _connection = factory.CreateConnection();
            _channel    = _connection.CreateModel();

            // Declare all exchanges upfront — idempotent, safe to call on every startup
            _channel.ExchangeDeclare(MembershipExchange, ExchangeType.Fanout, durable: true);
            _channel.ExchangeDeclare(TeamsExchange,      ExchangeType.Fanout, durable: true);
            _channel.ExchangeDeclare(ProjectsExchange,   ExchangeType.Fanout, durable: true);
        }

        public void Publish(MembershipChangedEvent evt)    => Send(MembershipExchange, evt);
        public void PublishTeam(TeamChangedEvent evt)       => Send(TeamsExchange, evt);
        public void PublishProject(ProjectChangedEvent evt) => Send(ProjectsExchange, evt);

        private void Send<T>(string exchange, T evt)
        {
            try
            {
                var body  = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));
                var props = _channel.CreateBasicProperties();
                props.Persistent = true;
                _channel.BasicPublish(exchange, routingKey: string.Empty, props, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish to exchange {Exchange}", exchange);
            }
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
