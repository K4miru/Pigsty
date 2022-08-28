using Microsoft.Extensions.DependencyInjection;
using Pigsty.CQRS.Command;
using Pigsty.CQRS.Query;
using Pigsty.Dispatcher;

namespace Pigsty.CQRS;

public static class Extensions
{
    public static IServiceCollection AddCQRS(this IServiceCollection services)
        => services.AddDispatcher()
                   .AddSingleton<IQueryDispatcher, QueryDispatcher>()
                   .AddSingleton<ICommandDispatcher, CommandDispatcher>();

    public static IServiceCollection AddAllQueryHandlers(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromEntryAssembly()
                    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection AddAllCommandHandlers(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        services.Scan(scan => scan.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        return services;
    }
}