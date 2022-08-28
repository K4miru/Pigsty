namespace Pigsty.MessagesBrokers;

[AttributeUsage(AttributeTargets.Class)]
public class MessageAttribute : Attribute
{
    public string ExchangeName { get; }

    public MessageAttribute(string exchangeName)
    {
        ExchangeName = exchangeName;
    }
}