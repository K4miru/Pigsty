using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pigsty.Dispatcher;
using Pigsty.Events;
using System.Reflection;
using System.Windows.Input;

namespace Pigsty.MessagesBrokers;

public static class Extensions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
        services.Configure<MessageBrokerConfiguration>(configuration?.GetSection("MessageBroker"));

        services.AddEventDispatcher();
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        services.AddTransient<IMessageBroker, MessageBroker>();
        services.AddSingleton<IMessageSubscriber, MessageSubscriber>();

        return services;
    }

    public static IServiceCollection AddAllEventSubscribers(this IServiceCollection services)
    {
        var messageSubscriber = services.BuildServiceProvider().GetService<IMessageSubscriber>();

        var types = Assembly.GetEntryAssembly()
                            .GetTypes()
                            .Where(p => typeof(IEvent).IsAssignableFrom(p) && p.IsClass);

        foreach (var type in types)
        {
            MethodInfo subscribe = typeof(IMessageSubscriber)?.GetMethod(nameof(IMessageSubscriber.Subscribe));
            MethodInfo typedSubscribe = subscribe.MakeGenericMethod(type);
            typedSubscribe.Invoke(messageSubscriber, null);
        }
        return services;
    }

    public static IServiceCollection AddAllCommandSubscribers(this IServiceCollection services)
    {
        var messageSubscriber = services.BuildServiceProvider().GetService<IMessageSubscriber>();

        var types = Assembly.GetEntryAssembly()
                            .GetTypes()
                            .Where(p => typeof(ICommand).IsAssignableFrom(p) && p.IsClass);

        foreach (var type in types)
        {
            MethodInfo subscribe = typeof(IMessageSubscriber)?.GetMethod(nameof(IMessageSubscriber.Subscribe));
            MethodInfo typedSubscribe = subscribe.MakeGenericMethod(type);
            typedSubscribe.Invoke(messageSubscriber, null);
        }
        return services;
    }
}