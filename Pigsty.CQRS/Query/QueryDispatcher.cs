using Pigsty.Dispatcher;

namespace Pigsty.CQRS.Query;

internal sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IDispatcher _dispatcher;

    public QueryDispatcher(IDispatcher dispatcher) => _dispatcher = dispatcher;

    public async Task<TOut> SendAsync<TIn, TOut>(TIn query, CancellationToken cancellationToken = default) where TIn : class, IQuery<TOut> where TOut : class
        => await _dispatcher.SendAsync<TIn, TOut>(query, cancellationToken);
}
