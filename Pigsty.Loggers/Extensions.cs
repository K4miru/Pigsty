using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Pigsty.Loggers;

public static class Extensions
{
    public static IServiceCollection AddMonitoring(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            .Enrich.WithCorrelationIdHeader(CorrelationIdConfiguration.HeaderName)
                            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {CorrelationId} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                            .CreateLogger();

        return services;
    }

    public static void UseMonitoring(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
    }
}