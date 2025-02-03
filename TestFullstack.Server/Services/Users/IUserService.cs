using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Models;

namespace TestFullstack.Server.Services.Users
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserAsync(ClaimsPrincipal user); 
        Task<IdentityResult> AddUserAsync(AddUserDto model);
        Task<IdentityResult> UpdateUserAsync(UpdateUserDto model);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IdentityResult> DeleteUserAsync(string userId);
        Task<bool> RoleExistsAsync(string role);
    }
}
