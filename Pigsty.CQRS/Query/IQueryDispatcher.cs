namespace Pigsty.CQRS.Query;

public interface IQueryDispatcher
{
    Task<TOut> SendAsync<TIn, TOut>(TIn dispatch, CancellationToken cancellationToken) where TIn : class, IQuery<TOut> where TOut : class;
}
