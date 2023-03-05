using Core.Lib.Authentication.Constants;
using Core.Lib.Authentication.Models;
using Core.Lib.Authentication.Services;
using Core.Lib.Ioc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        
        public AuthController()
        {
            _authService = IocContainer.Instance.Resolve<AuthService>();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogInAsync(LogInDto loginDto)
        {
            var logInResponse = await _authService.CanLogInAsync(loginDto);
            if (logInResponse.Status == ResponseStatus.Failed)
            {
                return Unauthorized(logInResponse);
            }
            return Ok(await _authService.LogInAsync(loginDto));
        }
        
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogOutAsync(LogOutDto logOutDto)
        {
            var logOutResponse = await _authService.LogOutAsync(logOutDto);
            return Ok(logOutResponse.Message);
        }

        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> GetRefreshTokenAsync(TokenDto tokenDto)
        {
            var refreshTokenResponse = await _authService.CanGetRefreshTokenAsync(tokenDto);
            if (refreshTokenResponse.Status == ResponseStatus.Failed)
            {
                return Unauthorized(refreshTokenResponse);
            }

            if (refreshTokenResponse.Status == ResponseStatus.Ignored)
            {
                return Ok(refreshTokenResponse);
            }
            return Ok(await _authService.GetRefreshTokenAsync(tokenDto));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(UserModel userModel)
        {
            return await Task.FromResult(Ok(await _authService.RegisterAsync(userModel)));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CheckAccess()
        {
            return await Task.FromResult(Ok("Can Access"));
        }
    }
}
