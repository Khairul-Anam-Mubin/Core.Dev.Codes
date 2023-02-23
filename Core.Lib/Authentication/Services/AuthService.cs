using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Lib.Authentication.Models;
using Core.Lib.Ioc;

namespace Core.Lib.Authentication.Services
{
    public class AuthService
    {
        private readonly TokenService _tokenService;
        private readonly UserService _userService;
        
        public AuthService()
        {
            _tokenService = new TokenService();
            _userService = IocContainer.Instance.Resolve<UserService>();
        }

        public async Task<TokenDto> GetTokenDtoAsync(LogInDto loginDto)
        {
            
            var claims = new List<Claim>();
            claims.Add(new Claim("Email", loginDto.Email));
            claims.Add(new Claim("UserName", loginDto.UserName));
            claims.Add(new Claim("jti", Guid.NewGuid().ToString()));
            
            var accessToken = _tokenService.GenerateJwtToken("SecretKey", "issuer", "audience", 100, claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            
            return await Task.FromResult(new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        public async Task<ResponseDto>CanLogInAsync(LogInDto logInDto)
        {
            var response = new ResponseDto();
            if (string.IsNullOrEmpty(logInDto.Email) || string.IsNullOrEmpty(logInDto.Password))
            {
                response.Status = "Failed";
                response.Message = "Email or Password not set";
                return response;
            }

            var canLogIn = await _userService.IsUserExistAsync(logInDto);
            if (canLogIn == false)
            {
                response.Status = "Failed";
                response.Status = "Email or Password not matched";
            }

            response.Status = "Success";
            response.Message = "Email and Password matched";

            return response;
        }

        public async Task<ResponseDto> RegisterAsync(UserModel userModel)
        {
            userModel.CreateGuidId();
            if (await _userService.CreateUserAsync(userModel))
            {
                return await Task.FromResult(new ResponseDto
                {
                    Message = "Register successfully",
                    Status = "Success"
                });
            }

            return await Task.FromResult(new ResponseDto
            {
                Message = "Register Error",
                Status = "Failed"
            });
        }
    }
}
