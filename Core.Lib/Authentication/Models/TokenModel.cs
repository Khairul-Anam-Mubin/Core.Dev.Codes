using Core.Lib.Database.Models;
namespace Core.Lib.Authentication.Models
{
    public class TokenModel : ARepositoryItem
    {
        public string AccessToken {get; set;}
        public string RefreshToken {get; set;}
        public string Email {get; set;}
        public DateTime CreatedAt {get; set;}
        public bool Suspicious {get; set;}
        
        public TokenDto ToTokenDto()
        {
            return new TokenDto
            {
                AccessToken = AccessToken,
                RefreshToken = RefreshToken
            };
        }
    }
}