using Pigsty.Dispatcher;

namespace Pigsty.CQRS.Command;

public interface ICommand<TOut> : IDispatch<TOut> where TOut : class { }
public interface ICommand : IDispatch { }