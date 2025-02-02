using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestFullstack.Server.Entities;
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
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, data) = await _authService.RegisterAsync(model.Email, model.Password);
            return success ? Ok(data) : BadRequest(message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
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

    //public class AuthController : ControllerBase
    //{
    //    private readonly UserManager<ApplicationUser> _userManager;
    //    private readonly SignInManager<ApplicationUser> _signInManager;
    //    private readonly IConfiguration _configuration;

    //    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    //    {
    //        _userManager = userManager;
    //        _signInManager = signInManager;
    //        _configuration = configuration;
    //    }

    //    [HttpPost("register")]
    //    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    //    {
    //        if (!ModelState.IsValid)
    //            return BadRequest(ModelState);

    //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
    //        var result = await _userManager.CreateAsync(user, model.Password);

    //        if (!result.Succeeded)
    //            return BadRequest(result.Errors);

    //        if (_userManager.Users.Count() <= 1)
    //            await _userManager.AddToRoleAsync(user, "Manager");
    //        else
    //            await _userManager.AddToRoleAsync(user, "Customer");

    //        var roles = await _userManager.GetRolesAsync(user);
    //        var token = GenerateJwtToken(user, roles);

    //        return Ok(new
    //        {
    //            userId = user.Id,
    //            token,
    //            email = user.Email,
    //            customerId = user.CustomerId,
    //            role = roles.FirstOrDefault()
    //        });
    //    }

    //    [HttpPost("login")]
    //    public async Task<IActionResult> Login([FromBody] LoginModel model)
    //    {
    //        if (!ModelState.IsValid)
    //            return BadRequest(ModelState);

    //        var user = await _userManager.FindByEmailAsync(model.Email);
    //        if (user == null)
    //            return Unauthorized("Invalid email or password.");

    //        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);


    //        if (!result.Succeeded)
    //        {
    //            return Unauthorized("Неверный email или пароль!");
    //        }

    //        var roles = await _userManager.GetRolesAsync(user);
    //        var token = GenerateJwtToken(user, roles);
    //        return Ok(new
    //        {
    //            userId = user.Id,
    //            token,
    //            email = user.Email,
    //            customerId = user.CustomerId,
    //            role = roles.FirstOrDefault()
    //        });
    //    }

    //    [HttpPost("logout")]
    //    public async Task<IActionResult> Logout()
    //    {
    //        await _signInManager.SignOutAsync();
    //        return Ok("Успешный выход из системы!");
    //    }


    //    private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
    //    {
    //        var claims = new List<Claim>
    //        {
    //            new Claim(ClaimTypes.NameIdentifier, user.Id),
    //            new Claim(ClaimTypes.Email, user.Email),
    //            new Claim(ClaimTypes.Name, user.UserName),
    //        };

    //        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //        var token = new JwtSecurityToken(
    //            issuer: _configuration["Jwt:Issuer"],
    //            audience: _configuration["Jwt:Audience"],
    //            claims: claims,
    //            expires: DateTime.Now.AddHours(6),
    //            signingCredentials: creds);

    //        return new JwtSecurityTokenHandler().WriteToken(token);
    //    }

    //    [HttpGet("role")]
    //    [Authorize]
    //    public async Task<IActionResult> GetUserRole()
    //    {
    //        var user = await _userManager.GetUserAsync(User);
    //        if (user == null) return Unauthorized();

    //        var roles = await _userManager.GetRolesAsync(user);
    //        return Ok(new { role = roles.FirstOrDefault() });
    //    }
    //}
}
