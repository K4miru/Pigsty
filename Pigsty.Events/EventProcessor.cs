using Pigsty.Events;

namespace Pigsty.Events;

internal sealed class EventProcessor : IEventProcessor
{
    private readonly IEventDispatcher _eventDispatcher;

    public EventProcessor(IEventDispatcher eventProcessor)
    {
        _eventDispatcher = eventProcessor;
    }

    public async Task SendAsync<TIn>(TIn @event, CancellationToken cancellationToken) where TIn : class, IEvent
    {

        await _eventDispatcher.SendAsync(@event, cancellationToken);
    }
}
