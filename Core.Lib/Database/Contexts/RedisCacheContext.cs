using Newtonsoft.Json;
using System.Composition;
using Core.Lib.Database.Interfaces;
using Core.Lib.Database.Models;
using Core.Lib.Ioc;
using MongoDB.Driver;

namespace Core.Lib.Database.Contexts
{
    [Export("RedisCacheContext", typeof(IRepositoryContext))]
    [Shared]
    public class RedisCacheContext : IRepositoryContext
    {
        private readonly IRedisCacheClient _redisClient;

        public RedisCacheContext()
        {
            //_redisClient = redisClient;
            _redisClient = IocContainer.Instance.Resolve<IRedisCacheClient>("RedisCacheClient");
        }

        public async Task<bool> InsertItemAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem
        {
            try
            {
                var database = _redisClient.GetDatabase(databaseInfo);
                var itemSerialized = JsonConvert.SerializeObject(item);
                await database.StringSetAsync(item.GetId(), itemSerialized);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Problem Insert Items");
                return false;
            }
        }

        public async Task<bool> UpdateItemAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem
        {
            try
            {
                var database = _redisClient.GetDatabase(databaseInfo);
                var itemSerialized = JsonConvert.SerializeObject(item);
                await database.StringSetAsync(item.GetId(), itemSerialized);
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
                var database = _redisClient.GetDatabase(databaseInfo);
                await database.KeyDeleteAsync(id);
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
                var database = _redisClient.GetDatabase(databaseInfo);
                var itemSerialized = await database.StringGetAsync(id);
                var item = JsonConvert.DeserializeObject<T>(itemSerialized);
                return item;
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
                var database = _redisClient.GetDatabase(databaseInfo);
                var itemsSerialized = await database.StringGetAsync(typeof(T).Name);
                var items = JsonConvert.DeserializeObject<List<T>>(itemsSerialized);
                return items;
            }
            catch (Exception)
            {
                Console.WriteLine("Problem Get Item");
                return null;
            }
        }

        Task<T> IRepositoryContext.GetItemByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteItemsByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition) where T : class, IRepositoryItem
        {
            throw new NotImplementedException();
        }
        public Task<bool> SaveItemAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem
        {
            throw new NotImplementedException();
        }
    }
}