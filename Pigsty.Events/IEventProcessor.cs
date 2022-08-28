namespace Pigsty.Events;

public interface IEventProcessor
{
    Task SendAsync<TIn>(TIn @event, CancellationToken cancellationToken) where TIn : class, IEvent;
}
