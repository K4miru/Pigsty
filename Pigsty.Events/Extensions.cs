using Microsoft.Extensions.DependencyInjection;
using Pigsty.Dispatcher;

namespace Pigsty.Events;

public static class Extensions
{
    public static IServiceCollection AddEventDispatcher(this IServiceCollection services)
        => services.AddDispatcher().AddSingleton<IEventDispatcher, EventDispatcher>();
    
    public static IServiceCollection AddAllEventHandlers(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromEntryAssembly()
                    .AddClasses(classes => classes.AssignableTo(typeof(IEventHandler<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }
}