using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Models;
using TestFullstack.Server.Services.Auth;

namespace TestFullstack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, data) = await _authService.RegisterAsync(model.Email, model.Password);
            return success ? Ok(data) : BadRequest(message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var (success, message, data) = await _authService.LoginAsync(model.Email, model.Password, model.RememberMe);
            return success ? Ok(data) : Unauthorized(message);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok("Успешный выход из системы!");
        }
    }
}
