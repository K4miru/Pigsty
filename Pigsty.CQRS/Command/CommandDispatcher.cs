using Pigsty.Dispatcher;
using Pigsty.MessagesBrokers;

namespace Pigsty.CQRS.Command;

internal sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IDispatcher _dispatcher;
    private readonly IMessageBroker _messageBroker;

    public CommandDispatcher(IDispatcher dispatcher, IMessageBroker messageBroker)
    {
        _dispatcher = dispatcher;
        _messageBroker = messageBroker;
    }

    public async Task SendAsync<TIn>(TIn command, CancellationToken cancellationToken = default) where TIn : class, ICommand
        => await _dispatcher.SendAsync(command, cancellationToken);

    public async Task<TOut> SendAsync<TIn, TOut>(TIn command, CancellationToken cancellationToken = default) where TIn : class, ICommand<TOut> where TOut : class
        => await _dispatcher.SendAsync<TIn, TOut>(command, cancellationToken);

    public async Task PublishAsync<TIn>(TIn command, CancellationToken cancellationToken) where TIn : class, ICommand
        => await _messageBroker.Publish(command);
}