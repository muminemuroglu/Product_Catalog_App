using MongoDB.Driver;

namespace ProductCatalogApp.utils
{
    public class DBMongo
    {
        private readonly string connectionString = "mongodb://localhost:27017";
        private readonly string databaseName = "ProductCatalog";
        private readonly IMongoDatabase _database;

        public DBMongo()
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

    }
}