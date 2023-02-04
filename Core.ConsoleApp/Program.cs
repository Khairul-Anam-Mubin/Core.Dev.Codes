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
            var insert = await mongoDbContext.InsertItemAsync<UserModel>(databaseInfo, (UserModel)user);
            Console.WriteLine($"Insert : {insert}");
            
            var getAndUpdateId = user.GetId();
            var get = await mongoDbContext.GetItemByIdAsync<UserModel>(databaseInfo, getAndUpdateId);
            var getString = JsonConvert.SerializeObject(get);
            Console.WriteLine($"Get : {getString}");
            
            get.Name = "Araf";
            get.Email = "aman.araf@gmail.com";
            var update = await mongoDbContext.UpdateItemAsync<UserModel>(databaseInfo, get);
            var updateString = JsonConvert.SerializeObject(get);
            Console.WriteLine($"Update : {update}");
            Console.WriteLine($"Update : {updateString}");

            var deleteId = user.GetId();
            var delete = await mongoDbContext.DeleteItemByIdAsync<UserModel>(databaseInfo, deleteId);
            Console.WriteLine($"Delete : {delete}");


            var gets = await mongoDbContext.GetItemsAsync<UserModel>(databaseInfo);
            foreach (var item in gets)
            {
                var itemString = JsonConvert.SerializeObject(item);
                Console.WriteLine($"Gets : {itemString}");
            }
        }
    }
}