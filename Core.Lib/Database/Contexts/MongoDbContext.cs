using Core.Lib.Database.Interfaces;
using Core.Lib.Database.Models;
using MongoDB.Driver;

namespace Core.Lib.Database.Contexts
{
    public class MongoDbContext : IRepositoryContext
    {
        private readonly IMongoDbClient _mongoDbClient;
        
        public MongoDbContext(IMongoDbClient mongoDbClient)
        {
            _mongoDbClient = mongoDbClient;
        }
        
        public async Task<bool> InsertItemAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem
        {
            try
            {
                var collection = _mongoDbClient.GetCollection<T>(databaseInfo);
                await collection.InsertOneAsync(item);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Problem Insert Item");
                return false;
            }
        }

        public async Task<bool> UpdateItemAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem
        {
            try
            {
                var collection = _mongoDbClient.GetCollection<T>(databaseInfo);
                var filter = Builders<T>.Filter.Eq("Id", item.GetId());
                await collection.ReplaceOneAsync(filter, item);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Problem Update Item");
                return false;
            }
        }
        
        public async Task<bool> DeleteItemByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IRepositoryItem
        {
            try
            {
                var collection = _mongoDbClient.GetCollection<T>(databaseInfo);
                var filter = Builders<T>.Filter.Eq("Id", id);
                await collection.DeleteOneAsync(filter);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Problem Delete Item");
                return false;
            }
        }

        public async Task<T> GetItemByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IRepositoryItem
        {
            try
            {
                var collection = _mongoDbClient.GetCollection<T>(databaseInfo);
                var filter = Builders<T>.Filter.Eq("Id", id);
                var items = await collection.FindAsync<T>(filter);
                return await items.FirstOrDefaultAsync<T>();
            }
            catch (Exception)
            {
                Console.WriteLine("Problem Get Item");
                return null;
            }
        }

        public async Task<List<T>> GetItemsAsync<T>(DatabaseInfo databaseInfo) where T : class, IRepositoryItem
        {
            try
            {
                var collection = _mongoDbClient.GetCollection<T>(databaseInfo);
                var filter = Builders<T>.Filter.Empty;
                var items = await collection.FindAsync<T>(filter);
                return await items.ToListAsync<T>();
            }
            catch (Exception)
            {
                Console.WriteLine("Problem Get Items");
                return null;
            }
        }
    }
}