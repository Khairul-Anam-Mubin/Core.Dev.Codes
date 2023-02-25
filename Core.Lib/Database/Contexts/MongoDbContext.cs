using System.Composition;
using Core.Lib.Database.DbClients;
using MongoDB.Driver;
using Core.Lib.Database.Interfaces;
using Core.Lib.Database.Models;
using Core.Lib.Ioc;
using Newtonsoft.Json;

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
           _mongoDbClient = IocContainer.Instance.Resolve<IMongoDbClient>();
        }
        
        public async Task<bool> InsertItemAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem
        {
            try
            {
                var collection = _mongoDbClient.GetCollection<T>(databaseInfo);
                await collection.InsertOneAsync(item);
                Console.WriteLine($"Insert Successfully, Item : {JsonConvert.SerializeObject(item)}\n");
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine($"Problem Insert Item : {JsonConvert.SerializeObject(item)}\n");
                return false;
            }
        }
        
        public async Task<bool> SaveItemAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem
        {
            try
            {
                var collection = _mongoDbClient.GetCollection<T>(databaseInfo);
                var filter = Builders<T>.Filter.Eq("Id", item.GetId());
                await collection.ReplaceOneAsync(filter, item, new ReplaceOptions {
                    IsUpsert = true
                });
                Console.WriteLine($"Successfully Save Item : {JsonConvert.SerializeObject(item)}\n");
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine($"Problem Save Item : {JsonConvert.SerializeObject(item)}\n");
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
                Console.WriteLine($"Successfully Update Item : {JsonConvert.SerializeObject(item)}\n");
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine($"Problem Update Item : {JsonConvert.SerializeObject(item)}\n");
                return false;
            }
        }
        
        public async Task<bool> DeleteItemByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IRepositoryItem
        {
            try
            {
                var collection = _mongoDbClient.GetCollection<T>(databaseInfo);
                var filter = Builders<T>.Filter.Eq("Id", id);
                var res = await collection.DeleteOneAsync(filter);
                Console.WriteLine($"Successfully Item Deleted, Id: {JsonConvert.SerializeObject(id)}\n");
                return res == null? false : res.DeletedCount > 0;
            }
            catch (Exception)
            {
                Console.WriteLine($"Problem Delete Item, Id : {JsonConvert.SerializeObject(id)}\n");
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
                var item = await items.FirstOrDefaultAsync<T>();
                Console.WriteLine($"Successfully Get Item : {JsonConvert.SerializeObject(item)}\n");
                return item;
            }
            catch (Exception)
            {
                Console.WriteLine($"Problem Get Item, id : {id}\n");
                return null;
            }
        }

        public async Task<List<T>> GetItemsAsync<T>(DatabaseInfo databaseInfo) where T : class, IRepositoryItem
        {
            try
            {
                var collection = _mongoDbClient.GetCollection<T>(databaseInfo);
                var filter = Builders<T>.Filter.Empty;
                var itemsCursor = await collection.FindAsync<T>(filter);
                var items = await itemsCursor.ToListAsync<T>();
                Console.WriteLine($"Successfully Get items, Count: {JsonConvert.SerializeObject(items.Count)}\n");
                return items;
            }
            catch (Exception)
            {
                Console.WriteLine($"Problem Get Items\n");
                return null;
            }
        }

        public async Task<T> GetItemByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition) where T : class, IRepositoryItem
        {
            try
            {
                var collection = _mongoDbClient.GetCollection<T>(databaseInfo);
                var items = await collection.FindAsync<T>(filterDefinition);
                var item = await items.FirstOrDefaultAsync<T>();
                 Console.WriteLine($"Successfully Get Item by filter : {JsonConvert.SerializeObject(item)}\n");
                return item;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Problem Get Item by fiter \n");
                return null;
            }
        }

        public async Task<bool> DeleteItemsByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition) where T : class, IRepositoryItem
        {
            try
            {
                var collection = _mongoDbClient.GetCollection<T>(databaseInfo);
                var res = await collection.DeleteManyAsync(filterDefinition);
                Console.WriteLine($"Successfully Delete Items, count : {JsonConvert.SerializeObject(res?.DeletedCount)}\n");
                return res == null? false : res.DeletedCount > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Problem Delete Items by fiter \n");
                return false;
            }
        }
    }
}