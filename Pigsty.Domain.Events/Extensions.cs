using Microsoft.Extensions.DependencyInjection;

namespace Pigsty.Domain.Events;

public static class Extensions
{
    public static IServiceCollection RegisterAllDomainEventHandlers(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromEntryAssembly()
                    .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }
}