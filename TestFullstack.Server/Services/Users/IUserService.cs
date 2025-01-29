using Microsoft.AspNetCore.Identity;
using TestFullstack.Server.Models;

namespace TestFullstack.Server.Services.Users
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<IdentityResult> AddUserAsync(RegisterModel model);
        Task<IdentityResult> AddToRoleAsync(string userId, string role);//
        Task<IdentityResult> RemoveFromRoleAsync(string userId, string role); //
        Task<IdentityResult> UpdateUserAsync(string userId, UpdateUserModel model);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IdentityResult> DeleteUserAsync(string userId);
    }
}
