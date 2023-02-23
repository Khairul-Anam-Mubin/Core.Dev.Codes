using System.Composition;
using Core.Lib.Database.DbClients;
using MongoDB.Driver;
using Core.Lib.Database.Interfaces;
using Core.Lib.Database.Models;
using Core.Lib.Ioc;

namespace Core.Lib.Database.Contexts
{
    [Export("MongoDbContext", typeof(IRepositoryContext))]
    [Shared]
    public class MongoDbContext : IRepositoryContext
    {
        private readonly IMongoDbClient _mongoDbClient;
        
        public MongoDbContext()
        {
           // _mongoDbClient = mongoDbClient;
           _mongoDbClient = new MongoDbClient();
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

        public async Task<T> GetItemByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition) where T : class, IRepositoryItem
        {
            try
            {
                var collection = _mongoDbClient.GetCollection<T>(databaseInfo);
                var item = await collection.FindAsync<T>(filterDefinition);
                return await item.FirstOrDefaultAsync<T>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}