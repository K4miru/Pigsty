using Pigsty.Dispatcher;

namespace Pigsty.CQRS.Command;

public interface ICommandHandler<TIn, TOut> : IDispatchHandler<TIn, TOut> 
    where TIn : class, ICommand<TOut> where TOut : class 
{
}

public interface ICommandHandler<TIn> : IDispatchHandler<TIn> 
    where TIn : class, ICommand 
{
}