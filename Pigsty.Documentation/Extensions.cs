using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.Processors;
using Pigsty.Documentation.Events;
using Pigsty.Events;
using System.Reflection;

namespace Pigsty.Documentation;

public static class Extensions
{
    public static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerDocument(settings =>
        {
            settings.Title = "Piggy";
            settings.Version = "1.2.1";

            var types = Assembly.GetEntryAssembly()
                            .GetTypes()
                            .Where(p => typeof(IEvent).IsAssignableFrom(p) && p.IsClass);

            foreach (var type in types)
            {
                var documentProcessor = Activator.CreateInstance(typeof(AddAdditionalTypeProcessor<>).MakeGenericType(type));
                settings.DocumentProcessors.Add(documentProcessor as IDocumentProcessor);
            }
        });

        return services;
    }

    public static IApplicationBuilder UseDocumentation(this IApplicationBuilder app)
    {
        app.UseOpenApi();
        app.UseSwaggerUi3();

        if(app is WebApplication webApp)
        {
            webApp.MapGet("/events", () => {
                var types = Assembly.GetEntryAssembly()
                                    .GetTypes()
                                    .Where(p => typeof(IEvent).IsAssignableFrom(p) && p.IsClass);

                return types.Select(type => new EventSchema(type.Name, type.GetProperties().Select(p => new EventFields(p.Name, p.PropertyType.Name))));
            });
        }

        return app;
    }
}