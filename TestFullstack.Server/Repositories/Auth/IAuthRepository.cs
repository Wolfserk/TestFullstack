using Microsoft.AspNetCore.Identity;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe);
        Task AddToRoleAsync(ApplicationUser user, string role);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task SignOutAsync();
    }
}
