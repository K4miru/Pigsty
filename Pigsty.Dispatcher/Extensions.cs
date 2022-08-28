using Microsoft.Extensions.DependencyInjection;

namespace Pigsty.Dispatcher;

public static class Extensions
{
    public static IServiceCollection AddDispatcher(this IServiceCollection services)
        => services.AddSingleton<IDispatcher, Dispatcher>();
}