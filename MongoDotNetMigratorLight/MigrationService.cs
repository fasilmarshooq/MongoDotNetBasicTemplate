using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Reflection;

namespace MongoDotNetMigratorLight
{
    public interface IMigrationService
    {
        Task MigrateAsync();
    }

    public class MigrationService : IMigrationService
    {
        private readonly IVersionService _versionService;
        private readonly Assembly _assemblyToScan;
        private readonly IMongoDatabase _database;
        private readonly ILogger _logger;

        public MigrationService(string connectionString, string dbName, IVersionService versionService, Assembly assemblyToScan, ILogger? logger = null)
        {
            _versionService = versionService;
            _assemblyToScan = assemblyToScan;
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
            if (logger == null)
            {
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                });

                logger = loggerFactory.CreateLogger<MigrationService>();
            }
            _logger = logger;
        }

        public async Task MigrateAsync()
        {
            // 1. List all classes in the specified assembly which implement IMigrate interface
            var migrationTypes = _assemblyToScan.GetTypes()
                .Where(t => typeof(IMigration).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            foreach (var migrationType in migrationTypes)
            {
                // 2. Get name of the class and check if version exists
                var migrationInstance = (IMigration)Activator.CreateInstance(migrationType);
                if (migrationInstance == null)
                {
                    _logger.LogError($"Failed to create instance of {migrationType.Name}");
                    continue;
                }
                var migrationName = migrationInstance.GetType().Name;
                var versionExists = await _versionService.VersionExists(migrationType.Name);
                _logger.LogInformation($"Migration {migrationType.Name} exists: {versionExists}");
                // 3. If version exists, skip; if not, perform migration
                if (!versionExists)
                {
                    await migrationInstance.MigrateAsync(_database);
                    _logger.LogInformation($"Migration {migrationType.Name} completed");
                    // After successful migration, you might want to insert a version document
                    await _versionService.InsertVersionAsync(migrationType.Name);
                    _logger.LogInformation($"Migration {migrationType.Name} version inserted");
                }
            }
        }
    }
}
