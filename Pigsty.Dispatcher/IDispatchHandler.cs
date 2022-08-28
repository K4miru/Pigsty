namespace Pigsty.Dispatcher;

public interface IDispatchHandler<TIn> where TIn : class, IDispatch
{
    Task HandleAsync(TIn dispatch, CancellationToken cancellationToken);
}

public interface IDispatchHandler<TIn, TOut> where TIn : class, IDispatch<TOut> where TOut : class
{
    Task<TOut> HandleAsync(TIn dispatch, CancellationToken cancellationToken);
}