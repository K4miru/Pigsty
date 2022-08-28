using Pigsty.Dispatcher;

namespace Pigsty.CQRS.Query;

public interface IQuery<TOut> : IDispatch<TOut> where TOut : class { }