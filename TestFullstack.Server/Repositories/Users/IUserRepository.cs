using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TestFullstack.Server.Models;

namespace TestFullstack.Server.Repositories.Users
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<ApplicationUser?> GetUserAsync(ClaimsPrincipal user);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role);
        Task<bool> RoleExistsAsync(string role);
    }
}
