using StackExchange.Redis;
using Core.Lib.Database.Interfaces;
using Core.Lib.Database.Models;

namespace Core.Lib.Database.DbClients
{
    public class RedisCacheClient : IRedisCacheClient
    {
        private Dictionary<string, IConnectionMultiplexer> _cacheClients;
        
        public RedisCacheClient()
        {
            _cacheClients = new Dictionary<string, IConnectionMultiplexer>();
        }
        
        public IConnectionMultiplexer RegisterDbClient(DatabaseInfo databaseInfo)
        {
            if (_cacheClients.ContainsKey(databaseInfo.ConnectionString))
            {
                return _cacheClients[databaseInfo.ConnectionString];
            }
            try
            {
                var client = ConnectionMultiplexer.Connect(databaseInfo.ConnectionString);
                _cacheClients.Add(databaseInfo.ConnectionString , client);
                return client;
            }
            catch (Exception)
            {
                var message = $"Client Creation Error. Connection string {databaseInfo.ConnectionString}";
                Console.WriteLine(message);
                throw new Exception(message);
            }
        }

        public IConnectionMultiplexer GetDbClient(DatabaseInfo databaseInfo)
        {
            return GetDbClient(databaseInfo.ConnectionString);
        }

        public IConnectionMultiplexer GetDbClient(string connectionString)
        {
            if (_cacheClients.ContainsKey(connectionString))
            {
                return _cacheClients[connectionString];
            }
            var message = $"Client not exist. Connection string {connectionString}";
            Console.WriteLine(message);
            return null;
        }

        public IDatabase GetDatabase(DatabaseInfo databaseInfo)
        {
            try
            {
                var client = GetDbClient(databaseInfo);
                var database = client.GetDatabase();
                return database;
            }
            catch (Exception)
            {
                Console.WriteLine("Get Database Error");
                return null;
            }
        }
    }
}