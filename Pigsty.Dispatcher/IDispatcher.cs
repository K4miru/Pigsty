namespace Pigsty.Dispatcher;

public interface IDispatcher
{
    Task SendAsync<TIn>(TIn dispatch, CancellationToken cancellationToken) where TIn : class, IDispatch;
    Task<TOut> SendAsync<TIn, TOut>(TIn dispatch, CancellationToken cancellationToken) where TIn : class, IDispatch<TOut> where TOut : class;
}