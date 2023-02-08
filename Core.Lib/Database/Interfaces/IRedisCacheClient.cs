using Core.Lib.Database.Models;
using StackExchange.Redis;

namespace Core.Lib.Database.Interfaces
{
    public interface IRedisCacheClient
    {
        IConnectionMultiplexer RegisterDbClient(DatabaseInfo databaseInfo);
        IConnectionMultiplexer GetDbClient(DatabaseInfo databaseInfo);
        IConnectionMultiplexer GetDbClient(string connectionString);
        IDatabase GetDatabase(DatabaseInfo databaseInfo);
    }
}