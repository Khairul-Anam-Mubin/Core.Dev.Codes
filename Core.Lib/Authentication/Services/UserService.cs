using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Lib.Authentication.Models;
using Core.Lib.Database.Contexts;
using Core.Lib.Database.Interfaces;
using Core.Lib.Database.Models;
using Core.Lib.Ioc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Core.Lib.Authentication.Services
{
    public class UserService
    {
        private readonly IRepositoryContext _repositoryContext;
        private readonly DatabaseInfo _databaseInfo;
        private readonly IMongoDbClient _mongoDbClient;
        
        public UserService(IConfiguration configuration)
        {
            _repositoryContext = IocContainer.Instance.Resolve<IRepositoryContext>("MongoDbContext");
            _mongoDbClient = IocContainer.Instance.Resolve<IMongoDbClient>();
            _databaseInfo = _databaseInfo = configuration.GetSection("DatabaseInfo").Get<DatabaseInfo>();
            _mongoDbClient.RegisterDbClient(_databaseInfo);
        }
        
        public async Task<bool> CreateUserAsync(UserModel userModel)
        {
            return await _repositoryContext.SaveItemAsync(_databaseInfo, userModel);
        }

        public async Task<bool> IsUserExistAsync(LogInDto logInDto)
        {
            var emailFilter = Builders<UserModel>.Filter.Eq("Email", logInDto.Email);
            var passwordFilter = Builders<UserModel>.Filter.Eq("Password", logInDto.Password);
            var filter = Builders<UserModel>.Filter.And(emailFilter, passwordFilter);
            var userModel = await _repositoryContext.GetItemByFilterDefinitionAsync(_databaseInfo, filter);
            if (userModel != null)
            {
                return true;
            }
            return false;
        }

    }
}
