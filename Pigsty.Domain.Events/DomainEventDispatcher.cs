using Pigsty.Dispatcher;

namespace Pigsty.Domain.Events;

internal sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IDispatcher _dispatcher;

    public DomainEventDispatcher(IDispatcher dispatcher) => _dispatcher = dispatcher;

    public async Task SendAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IDomainEvent
        => await _dispatcher.SendAsync(@event, cancellationToken);
}
