using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestFullstack.Server.Entities;
using TestFullstack.Server.Repositories.Auth;

namespace TestFullstack.Server.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Message, object Data)> RegisterAsync(string email, string password)
        {
            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _authRepository.CreateUserAsync(user, password);

            if (!result.Succeeded)
                return (false, "Ошибка при регистрации", result.Errors);

            var role = await _authRepository.GetRolesAsync(user);
            if (!role.Any())
            {
                role = new List<string> { _authRepository.FindByEmailAsync(email).Result.Id == "1" ? "Manager" : "Customer" };
                await _authRepository.AddToRoleAsync(user, role.First());
            }

            var token = await GenerateJwtToken(user, role);

            return (true, "Регистрация успешна", new
            {
                userId = user.Id,
                token,
                email = user.Email,
                customerId = user.CustomerId,
                role = role.FirstOrDefault()
            });
        }

        public async Task<(bool Success, string Message, object Data)> LoginAsync(string email, string password, bool rememberMe)
        {
            var user = await _authRepository.FindByEmailAsync(email);
            if (user == null)
                return (false, "Неверный email или пароль", null)!;

            var result = await _authRepository.PasswordSignInAsync(email, password, rememberMe);
            if (!result.Succeeded)
                return (false, "Неверный email или пароль", null)!;

            var roles = await _authRepository.GetRolesAsync(user);
            var token = await GenerateJwtToken(user, roles);

            return (true, "Вход выполнен", new
            {
                userId = user.Id,
                token,
                email = user.Email,
                customerId = user.CustomerId,
                role = roles.FirstOrDefault()
            });
        }

        public async Task LogoutAsync()
        {
            await _authRepository.SignOutAsync();
        }

        public async Task<string> GetUserRoleAsync(string userId)
        {
            var user = await _authRepository.FindByIdAsync(userId);
            if (user == null) return null!;

            var roles = await _authRepository.GetRolesAsync(user);
            return roles.FirstOrDefault()!;
        }

        public async Task<string> GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
