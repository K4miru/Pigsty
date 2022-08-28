using Pigsty.Dispatcher;

namespace Pigsty.MessagesBrokers;

public interface IMessageBroker
{
    Task Publish<T>(T @event) where T : class, IDispatch;
}