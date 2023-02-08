using Core.Lib.Database.Interfaces;
using Core.Lib.Database.Models;
using Newtonsoft.Json;

namespace Core.Lib.Database.Contexts
{
    public class RedisCacheContext : IRepositoryContext
    {
        private readonly IRedisCacheClient _redisClient;

        public RedisCacheContext(IRedisCacheClient redisClient)
        {
            _redisClient = redisClient;
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
    }
}