using Pigsty.Dispatcher;

namespace Pigsty.MessagesBrokers;

public interface IMessageSubscriber
{
    IMessageSubscriber Subscribe<T>() where T : class, IDispatch;
}