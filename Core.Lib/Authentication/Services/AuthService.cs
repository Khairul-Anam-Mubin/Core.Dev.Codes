using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Lib.Authentication.Helpers;
using Core.Lib.Authentication.Models;
using Core.Lib.Authentication.Repository;
using Core.Lib.Ioc;

namespace Core.Lib.Authentication.Services
{
    public class AuthService
    {
        private readonly TokenService _tokenService;
        private readonly UserService _userService;
        private readonly TokenHelper _tokenHelper;
        private readonly AuthRepository _authRepository;

        public AuthService()
        {
            _tokenService = IocContainer.Instance.Resolve<TokenService>();
            _userService = IocContainer.Instance.Resolve<UserService>();
            _tokenHelper = IocContainer.Instance.Resolve<TokenHelper>();
            _authRepository = IocContainer.Instance.Resolve<AuthRepository>();
        }

        public async Task<TokenDto> GetTokenDtoAsync(LogInDto loginDto)
        {
            
            var claims = new List<Claim>();
            claims.Add(new Claim("Email", loginDto.Email));
            claims.Add(new Claim("UserName", loginDto.UserName));
            claims.Add(new Claim("jti", Guid.NewGuid().ToString()));
            
            var accessToken = _tokenHelper.GenerateJwtToken("SecretKey", "issuer", "audience", 100, claims);
            var refreshToken = _tokenHelper.GenerateRefreshToken();
            var email = loginDto.Email;
            
            var tokenModel = new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Email = email,
                CreatedAt = DateTime.Now
            };
            tokenModel.CreateGuidId();
            
            await _authRepository.SaveTokenModelAsync(tokenModel);

            return await Task.FromResult(tokenModel.ToTokenDto());
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
            response.Message = "Login Successfully";

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

        public async Task<TokenDto> GetRefreshTokenAsync(TokenDto tokenDto)
        {
            var tokenModel = await _authRepository.GetTokenModelByRefreshTokenAsync(tokenDto.RefreshToken);

            if (tokenModel != null)
            {
                if (tokenModel.Suspicious == true)
                {
                    return new TokenDto
                    {
                        Status = "Failed",
                        Message = "Token Suspicious"
                    };
                }
                // change refresh and access token
                await _authRepository.SaveTokenModelAsync(tokenModel);
                return tokenModel.ToTokenDto();
            }
            return new TokenDto
            {
                Status = "Failed",
                Message = "Refresh token already invalidated"
            };
        }
    }
}
