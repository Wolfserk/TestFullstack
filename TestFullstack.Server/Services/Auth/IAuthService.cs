using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Services.Auth
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, object Data)> RegisterAsync(string email, string password);
        Task<(bool Success, string Message, object Data)> LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
        Task<string> GenerateJwtToken(ApplicationUser user, IList<string> roles);
        Task<string> GetUserRoleAsync(string userId);
    }
}
