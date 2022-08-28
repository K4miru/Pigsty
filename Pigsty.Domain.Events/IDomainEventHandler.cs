using Pigsty.Dispatcher;

namespace Pigsty.Domain.Events;

public interface IDomainEventHandler<T> : IDispatchHandler<T> where T : class, IDomainEvent { }