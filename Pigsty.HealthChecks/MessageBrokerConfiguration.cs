namespace Pigsty.HealthChecks;

internal sealed class MessageBrokerConfiguration
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string HostName { get; set; }
    public int Port { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
    public bool ExchangeDurable { get; set; }
    public bool ExchangeAutoDelete { get; set; }
    public bool QueueDurable { get; set; }
    public bool QueueExclusive { get; set; }
    public bool QueueAutoDelete { get; set; }
}