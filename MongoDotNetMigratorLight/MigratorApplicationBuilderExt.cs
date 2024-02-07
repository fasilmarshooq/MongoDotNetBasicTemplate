using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MongoDotNetMigratorLight
{
    public static class MigratorApplicationBuilderExt
    {

        public static async Task<IApplicationBuilder> RunMigration(this IApplicationBuilder app)
        {
            // Resolve the IMigrationService from the service provider
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationService>();
                // Run migrations
                await migrationService.MigrateAsync();
            }

            return app;
        }
    }
}
