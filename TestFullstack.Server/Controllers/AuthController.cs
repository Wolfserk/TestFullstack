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

namespace TestFullstack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (_userManager.Users.Count() <= 1)  //Админка первому пользователю (Можно закомментировать весть if после регистрации первого аккаунта)
                    await _userManager.AddToRoleAsync(user, "Manager");
                else
                    await _userManager.AddToRoleAsync(user, "Customer");
            }
            else
                return BadRequest(result.Errors);

            return Ok("Регистрация прошла успешно!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);


            if (!result.Succeeded)
            {
                Console.WriteLine($"Login failed for user {model.Email}. Reason: {result}");
                return Unauthorized("Неверный email или пароль!");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            //return Ok("Авторизация выполнена успешно");
            return Ok(new
            {
                userId = user.Id,
                token,
                email = user.Email,
                //customerId = user.CustomerId != null ? user.CustomerId : null,
                customerId = user.CustomerId,
                role = roles.FirstOrDefault()
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Успешный выход из системы!");
        }


        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sG2f3v5x8y/B1A2C3D4E5F6G7H8I9J0K1L2M3N4O5P6Q7R8S9T0U1V2W3X4"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7034",
                audience: "https://localhost:54305",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);
           
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //[HttpPost("token")]
        //public async Task<IActionResult> GenerateToken([FromBody] LoginModel model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
        //        return Unauthorized("Недействительные учетные данные");

        //    var authClaims = new[]
        //    {
        //        new Claim(ClaimTypes.Name, user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //    };

        //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        expires: DateTime.Now.AddHours(3),
        //        claims: authClaims,
        //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //    );

        //    return Ok(new
        //    {
        //        token = new JwtSecurityTokenHandler().WriteToken(token),
        //        expiration = token.ValidTo
        //    });
        //}
        [HttpGet("role")]
        [Authorize]
        public async Task<IActionResult> GetUserRole()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new { role = roles.FirstOrDefault() });
        }
    }
}
