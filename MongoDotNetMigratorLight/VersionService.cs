using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDotNetMigratorLight
{
    public class VersionInfo
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public DateTime ExecutedDate { get; set; }
    }

    public class VersionService : IVersionService
    {
        private readonly IMongoDatabase _database;

        public VersionService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<VersionInfo?> GetlatestVersion()
        {
            const string collectionName = "versions";


            var collectionExists = await CollectionExistsAsync(collectionName);
            if (!collectionExists)
            {
                await _database.CreateCollectionAsync(collectionName);
            }

            var versionsCollection = _database.GetCollection<VersionInfo>(collectionName);

            var latestVersion = await versionsCollection.Find(new BsonDocument())
                .SortByDescending(v => v.Id)
                .FirstOrDefaultAsync();

            return latestVersion;
        }

        public async Task InsertVersionAsync(string versionName)
        {
            const string collectionName = "versions";

            var collectionExists = await CollectionExistsAsync(collectionName);
            if (!collectionExists)
            {
                await _database.CreateCollectionAsync(collectionName);
            }

            var versionsCollection = _database.GetCollection<VersionInfo>(collectionName);

            var version = new VersionInfo
            {
                Name = versionName,
                ExecutedDate = DateTime.UtcNow
            };

            await versionsCollection.InsertOneAsync(version);
        }

        public async Task<bool> VersionExists(string name)
        {
            const string collectionName = "versions";

            var collectionExists = await CollectionExistsAsync(collectionName);
            if (!collectionExists)
            {
                return false;
            }

            var versionsCollection = _database.GetCollection<VersionInfo>(collectionName);

            var version = await versionsCollection.Find(v => v.Name == name).FirstOrDefaultAsync();

            return version != null;
        }

        private async Task<bool> CollectionExistsAsync(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collections = await _database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
            return await collections.AnyAsync();
        }
    }
}