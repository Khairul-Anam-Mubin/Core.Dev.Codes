using Core.Lib.Database.Interfaces;
using Core.Lib.Database.Models;

namespace Core.Lib.Database.Contexts
{
    public class MongoDbContext : IRepositoryContext
    {
        public Task<bool> InsertItemAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem
        {
            throw new NotImplementedException();
        }
        
        public Task<bool> DeleteItemByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IRepositoryItem
        {
            throw new NotImplementedException();
        }

        public Task<T> GetItemByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IRepositoryItem
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetItemsAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IRepositoryItem
        {
            throw new NotImplementedException();
        }
    }
}