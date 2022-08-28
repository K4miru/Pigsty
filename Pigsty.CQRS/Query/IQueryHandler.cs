using Pigsty.Dispatcher;

namespace Pigsty.CQRS.Query;

public interface IQueryHandler<TIn, TOut> : IDispatchHandler<TIn, TOut> 
    where TIn : class, IQuery<TOut> where TOut : class 
{
    new Task<TOut> HandleAsync(TIn query, CancellationToken cancellationToken);
}