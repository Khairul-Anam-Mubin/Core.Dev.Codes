using Core.Lib.Database.Models;
using Core.Lib.Database.Contexts;
using Core.Lib.Database.DbClients;
using Core.Lib.Database.Interfaces;
using Core.ConsoleApp.Models;
using Newtonsoft.Json;

namespace Core.ConsoleApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello");
            
            var databaseInfo = new DatabaseInfo();
            databaseInfo.ConnectionString = "mongodb://localhost:27017";
            databaseInfo.DatabaseName = "TestLibDb";


            IMongoDbClient mongoDbClient = new MongoDbClient();
            var dbClient = mongoDbClient.RegisterDbClient(databaseInfo);

            IRepositoryContext mongoDbContext = new MongoDbContext(mongoDbClient);

            var user = new UserModel();
            user.CreateGuidId();
            user.Name = "Mubin";
            user.Email = "anam.mubin1999@gmail.com";

            IRedisCacheClient redis = new RedisCacheClient();
            var redisDb = new DatabaseInfo();
            redisDb.DatabaseName = "RedisDb";
            redisDb.ConnectionString = "127.0.0.1:6379";
            databaseInfo = redisDb;
            redis.RegisterDbClient(redisDb);

            var redisDbContext = new RedisCacheContext(redis);
            await redisDbContext.InsertItemAsync<UserModel>(redisDb, user);
            
            var redisUser = await redisDbContext.GetItemByIdAsync<UserModel>(redisDb, user.Id);
            var ser = JsonConvert.SerializeObject(redisUser);
            Console.WriteLine($"Redis User: {ser}");

            var insert = await redisDbContext.InsertItemAsync<UserModel>(databaseInfo, (UserModel)user);
            Console.WriteLine($"Redis Insert : {insert}");
            
            var getAndUpdateId = user.GetId();
            var get = await redisDbContext.GetItemByIdAsync<UserModel>(databaseInfo, getAndUpdateId);
            var getString = JsonConvert.SerializeObject(get);
            Console.WriteLine($"Redis Get : {getString}");
            
            get.Name = "Araf";
            get.Email = "aman.araf@gmail.com";
            var update = await redisDbContext.UpdateItemAsync<UserModel>(databaseInfo, get);
            var updateString = JsonConvert.SerializeObject(get);
            Console.WriteLine($"Redis Update : {update}");
            Console.WriteLine($"Redis Update : {updateString}");

            var deleteId = user.GetId();
            var delete = await redisDbContext.DeleteItemByIdAsync<UserModel>(databaseInfo, deleteId);
            Console.WriteLine($"Redis Delete : {delete}");
            
        }
    }
}