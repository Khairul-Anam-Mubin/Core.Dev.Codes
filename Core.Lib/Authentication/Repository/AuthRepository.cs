using Core.Lib.Authentication.Models;
using Core.Lib.Database.Interfaces;
using Core.Lib.Database.Models;
using Core.Lib.Ioc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Core.Lib.Authentication.Repository
{
    public class AuthRepository
    {
        private readonly DatabaseInfo _databaseInfo;
        private readonly IMongoDbClient _mongoDbClient;
        private readonly IRepositoryContext _context;

        public AuthRepository(IConfiguration configuration)
        {
            _databaseInfo = configuration.GetValue<DatabaseInfo>("DatabaseInfo");
            _mongoDbClient = IocContainer.Instance.Resolve<IMongoDbClient>();
            _context = IocContainer.Instance.Resolve<IRepositoryContext>("MongoDbContext");
            _mongoDbClient.RegisterDbClient(_databaseInfo);
        }

        public async Task<bool> SaveTokenModelAsync(TokenModel tokenModel)
        {
           return await _context.InsertItemAsync(_databaseInfo, tokenModel);
        }

        public async Task<TokenModel> GetTokenModelByRefreshTokenAsync(string refreshToken)
        {
            var filter = Builders<TokenModel>.Filter.Eq("RefreshToken", refreshToken);
            var tokenModel = await _context.GetItemByFilterDefinitionAsync<TokenModel>(_databaseInfo, filter);
            return tokenModel;
        }
    }
}