using Core.Lib.Database.Models;
using MongoDB.Driver;

namespace Core.Lib.Database.Interfaces
{
    public interface IMongoDbClient
    {
        MongoClient RegisterDbClient(DatabaseInfo databaseInfo);
        MongoClient GetDbClient(DatabaseInfo databaseInfo);
        MongoClient GetDbClient(string connectionString);
        IMongoDatabase GetDatabase(DatabaseInfo databaseInfo);
        IMongoCollection<T> GetCollection<T>(DatabaseInfo databaseInfo);
    }
}