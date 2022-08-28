using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Pigsty.Databases.MongoDb;

public static class Extensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        var defaultConfiguration = configuration.GetConnectionString("DefaultConnection");
        var mongoClient = new MongoClient(defaultConfiguration);

        services.AddSingleton<IMongoClient>(mongoClient);

        services.AddTransient(sp =>
        {
            var databaseName = MongoUrl.Create(defaultConfiguration).DatabaseName;
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });

        services.AddScoped(typeof(IMongoDbRepository<,>), typeof(MongoDbRepository<,>));

        return services;
    }

    public static IApplicationBuilder UseMongoDb(this IApplicationBuilder app)
    {
        return app;
    }
}