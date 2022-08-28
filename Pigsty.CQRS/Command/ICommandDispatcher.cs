namespace Pigsty.CQRS.Command;

public interface ICommandDispatcher
{
    Task PublishAsync<TIn>(TIn command, CancellationToken cancellationToken) where TIn : class, ICommand;
    Task SendAsync<TIn>(TIn command, CancellationToken cancellationToken) where TIn : class, ICommand;
    Task<TOut> SendAsync<TIn, TOut>(TIn command, CancellationToken cancellationToken) where TIn : class, ICommand<TOut> where TOut : class;
}
