namespace Pigsty.Events;

public interface IEventDispatcher
{
    Task SendAsync<TIn>(TIn @event, CancellationToken cancellationToken) where TIn : class, IEvent;
}
