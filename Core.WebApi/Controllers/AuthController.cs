using Core.Lib.Authentication.Models;
using Core.Lib.Authentication.Services;
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
            _authService = new AuthService();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogInAsync(LogInDto loginDto)
        {
            var logInResponse = await _authService.CanLogInAsync(loginDto);
            if (logInResponse.Status == "Failed")
            {
                return Ok(logInResponse);
            }
            return Ok(await _authService.GetTokenDtoAsync(loginDto));
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(UserModel userModel)
        {
            return await Task.FromResult(Ok(await _authService.RegisterAsync(userModel)));
        }

        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> GetRefreshTokenAsync(TokenDto tokenDto)
        {
            return await Task.FromResult(Ok(await _authService.GetRefreshTokenAsync(tokenDto)));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CheckAccess()
        {
            return await Task.FromResult(Ok("Can Access"));
        }
    }
}
