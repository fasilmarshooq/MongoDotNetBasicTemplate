using MongoDB.Bson;
using MongoDB.Driver;
using MongoDotNetMigratorLight;

namespace MongoDbCrudApp.Data.Migrations
{
    public class InitMigration : IMigration
    {
        public Task MigrateAsync(IMongoDatabase database)
        {
            // insert dummy document into Student collection
            var collection = database.GetCollection<BsonDocument>("Students");
            var student = new BsonDocument
            {
                {"Name", "John Doe"},
                {"Department", "Computer Science"},
                {"Email", ""}
            };
            return collection.InsertOneAsync(student);
        }
    }
}
