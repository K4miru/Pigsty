namespace Pigsty.Domain.Events;

public interface IDomainEventDispatcher
{
    Task SendAsync<T>(T @event, CancellationToken cancellationToken) where T : class, IDomainEvent;
}
