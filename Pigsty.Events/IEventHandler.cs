using Pigsty.Dispatcher;

namespace Pigsty.Events;

public interface IEventHandler<TIn> : IDispatchHandler<TIn> where TIn : class, IEvent 
{
    Task HandleAsync(TIn @event, CancellationToken cancellationToken);
}