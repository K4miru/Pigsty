using Pigsty.Dispatcher;

namespace Pigsty.Events;

internal sealed class EventDispatcher : IEventDispatcher
{
    private readonly IDispatcher _dispatcher;

    public EventDispatcher(IDispatcher dispatcher) => _dispatcher = dispatcher;

    public async Task SendAsync<TIn>(TIn @event, CancellationToken cancellationToken = default) where TIn : class, IEvent
        => await _dispatcher.SendAsync(@event, cancellationToken);
}
