using Core.Lib.Database.Models;

namespace Core.Lib.Database.Interfaces
{
    public interface IRepositoryContext
    {
        Task<bool> InsertItemAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem;
        Task<bool> UpdateItemAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem;
        Task<bool> DeleteItemByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IRepositoryItem;
        Task<T> GetItemByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IRepositoryItem;
        Task<List<T>> GetItemsAsync<T>(DatabaseInfo databaseInfo) where T : class, IRepositoryItem;
    }
}