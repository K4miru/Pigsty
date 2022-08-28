using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Pigsty.Dispatcher;
using Pigsty.Loggers;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Pigsty.MessagesBrokers;

internal sealed class MessageBroker : IMessageBroker
{
    private readonly MessageBrokerConfiguration _rabbitConfiguration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private IModel _channel;
    public MessageBroker(
        IOptions<MessageBrokerConfiguration> rabbitConfiguration,
        IHttpContextAccessor httpContextAccessor)
    {
        _rabbitConfiguration = rabbitConfiguration.Value;
        _httpContextAccessor = httpContextAccessor;
        Initialize();
    }

    private void Initialize()
    {
        var connectionFactory = new ConnectionFactory()
        {
            UserName = _rabbitConfiguration.UserName,
            Password = _rabbitConfiguration.Password,
            HostName = _rabbitConfiguration.HostName,
            Port = _rabbitConfiguration.Port
        };
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();

        _channel.ExchangeDeclare(
            _rabbitConfiguration.ExchangeName,
            _rabbitConfiguration.ExchangeType,
            _rabbitConfiguration.ExchangeDurable,
            _rabbitConfiguration.ExchangeAutoDelete);
    }

    public Task Publish<T>(T @event) where T : class, IDispatch
    {
        var serializedJson = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(serializedJson);

        var queueName = $"{_rabbitConfiguration.ExchangeName.ToSnakeCase()}/{@event.GetType().Name.ToSnakeCase()}";
        var routingKey = @event.GetType().Name.ToSnakeCase();

        _channel.QueueDeclare(
            queueName,
            _rabbitConfiguration.QueueDurable,
            _rabbitConfiguration.QueueExclusive,
            _rabbitConfiguration.QueueAutoDelete);

        _channel.QueueBind(queueName, _rabbitConfiguration.ExchangeName, routingKey);

        var properties = _channel.CreateBasicProperties();
        properties.CorrelationId = _httpContextAccessor.HttpContext?.Request.Headers[CorrelationIdConfiguration.HeaderName];

        _channel.BasicPublish(exchange: _rabbitConfiguration.ExchangeName,
                              routingKey: routingKey,
                              basicProperties: properties,
                              body: body);

        return Task.CompletedTask;
    }
}