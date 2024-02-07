using MongoDB.Driver;

namespace MongoDotNetMigratorLight
{
    public interface IMigration
    {
        Task MigrateAsync(IMongoDatabase database);
    }



}
