using Microsoft.Extensions.DependencyInjection;

namespace Pigsty.Dispatcher;

internal sealed class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync<TIn>(TIn dispatch, CancellationToken cancellationToken = default) where TIn : class, IDispatch
    {
        using var scope = _serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IDispatchHandler<TIn>>();
        foreach (var handler in handlers)
        {
            await handler.HandleAsync(dispatch, cancellationToken);
        }
    }

    public async Task<TOut> SendAsync<TIn, TOut>(TIn dispatch, CancellationToken cancellationToken) where TIn : class, IDispatch<TOut> where TOut : class
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetService<IDispatchHandler<TIn, TOut>>();
        return await handler?.HandleAsync(dispatch, cancellationToken);
    }
}
