using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Pigsty.HealthChecks;

public static class Extensions
{
    private const string _healthCheckEndpoint = "/healthchecks";
    public static IServiceCollection AddAllHealthChecks(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
        var _rabbitConfiguration = new MessageBrokerConfiguration();
        configuration?.GetSection("MessageBroker").Bind(_rabbitConfiguration);

        var connectionFactory = new ConnectionFactory()
        {
            Uri = new Uri($"amqp://{_rabbitConfiguration.UserName}:{_rabbitConfiguration.Password}@{_rabbitConfiguration.HostName}:{_rabbitConfiguration.Port}"),
            AutomaticRecoveryEnabled = true
        };

        var defaultConfiguration = configuration.GetConnectionString("DefaultConnection");

        services.AddSingleton(connectionFactory.CreateConnection())
                .AddHealthChecksUI(setupSettings: setup =>
                {
                    setup.AddHealthCheckEndpoint("Basic health checks", _healthCheckEndpoint);
                })
                .AddInMemoryStorage();

        services.AddHealthChecks()
                .AddRabbitMQ()
                .AddMongoDb(defaultConfiguration);

        return services;
    }

    public static IApplicationBuilder UseAllHealthChecks(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(config => config.MapHealthChecksUI());
        if (app is WebApplication webApp)
        {
            webApp.MapHealthChecks(_healthCheckEndpoint, new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
        return app;
    }
}