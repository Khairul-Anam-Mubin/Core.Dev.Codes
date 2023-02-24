using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Core.Lib.Authentication.Helpers
{
    public class TokenHelper
    {
        public string GenerateJwtToken(string secretKey, string issuer, string audience, int expiredTimeInSec, List<Claim> claims = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiredTime = DateTime.Now.AddSeconds(expiredTimeInSec);
            var tokenOptions = new JwtSecurityToken(
                issuer : issuer, 
                audience : audience, 
                claims : claims, 
                expires : expiredTime, 
                signingCredentials: signingCredentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
        public List<Claim> GetClaims(string token)
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwtSecurityToken.Claims.ToList();
        }
        public string GetClaimType(Claim claim)
        {
            return claim.Type;
        }
        public string GetClaimValue(Claim claim)
        {
            return claim.Value;
        }
        public bool IsTokenExpired(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return true;
            }
            var jwtToken = new JwtSecurityToken(token);
            return (jwtToken == null) || (jwtToken.ValidFrom > DateTime.UtcNow) || (jwtToken.ValidTo < DateTime.UtcNow);
        }
    }
}