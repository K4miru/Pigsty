using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pigsty.Dispatcher;
using Pigsty.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog.Context;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Pigsty.MessagesBrokers;

internal sealed class MessageSubscriber : IMessageSubscriber
{
    private readonly MessageBrokerConfiguration _rabbitConfiguration;
    private readonly IDispatcher _eventDispatcher;
    private IModel _channel;
    private ILogger<MessageSubscriber> _logger;
    public MessageSubscriber(
        IOptions<MessageBrokerConfiguration> rabbitConfiguration,
        ILogger<MessageSubscriber> logger,
        IDispatcher mediator)
    {
        _rabbitConfiguration = rabbitConfiguration.Value;
        _logger = logger;
        _eventDispatcher = mediator;
        Initialize();
    }

    private void Initialize()
    {
        var connectionFactory = new ConnectionFactory()
        {
            DispatchConsumersAsync = true,
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

    public IMessageSubscriber Subscribe<T>() where T : class, IDispatch
    {
        var queueName = $"{_rabbitConfiguration.ExchangeName.ToSnakeCase()}/{typeof(T).Name.ToSnakeCase()}";
        var routingKey = typeof(T).Name.ToSnakeCase();

        _channel.QueueDeclare(
            queueName,
            _rabbitConfiguration.QueueDurable,
            _rabbitConfiguration.QueueExclusive,
            _rabbitConfiguration.QueueAutoDelete);

        var exchangeName = typeof(T).GetCustomAttribute<MessageAttribute>()?.ExchangeName ?? _rabbitConfiguration.ExchangeName;

        _channel.QueueBind(queueName, exchangeName, routingKey);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, args) =>
        {
            using (LogContext.PushProperty("CorrelationId", args.BasicProperties.CorrelationId))
            {
                _logger.LogInformation("Message recived");
                try
                {
                    _logger.LogInformation("Message processing started");
                    var payload = Encoding.UTF8.GetString(args.Body.ToArray());
                    var @event = JsonSerializer.Deserialize<T>(payload);
                    await _eventDispatcher.SendAsync(@event, new CancellationToken());

                    _channel.BasicAck(args.DeliveryTag, false);
                    _logger.LogInformation("Message processing finished");
                }
                catch (Exception)
                {
                    _logger.LogWarning("Message processing failed");
                    _channel.BasicAck(args.DeliveryTag, false);
                    throw;
                }
                _logger.LogInformation("Message consumed");
                await Task.Yield();
            }
        };

        _channel.BasicConsume(queueName, false, consumer);
        return this;
    }
}