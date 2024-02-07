using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Reflection;

namespace MongoDotNetMigratorLight
{
    public static class MigratorServiceCollectionExt
    {
        public static IServiceCollection AddMongoNetMigratorLight(
        this IServiceCollection services,
        string mongoConnectionString,
        string databaseName,
        Assembly migrationAssembly)
        {

            // Register IMongoClient
            services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));

            // Register IMongoDatabase
            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });

            // Register IVersionService
            services.AddScoped<IVersionService, VersionService>();

            // Register IMigrationService and pass the migration assembly
            services.AddScoped<IMigrationService>(sp =>
            {
                var versionService = sp.GetRequiredService<IVersionService>();
                var logger = sp.GetService<ILogger<MigrationService>>();

                return new MigrationService(mongoConnectionString, databaseName, versionService, migrationAssembly, logger);
            });

            return services;
        }

    }
}
