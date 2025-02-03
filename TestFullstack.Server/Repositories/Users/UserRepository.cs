using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser?> GetUserAsync(ClaimsPrincipal user) // ✅ Реализация метода
        {
            return await _userManager.GetUserAsync(user);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        //public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        //{
        //    Console.WriteLine(user.Id);
        //    return await _userManager.UpdateAsync(user);
        //}

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                Console.WriteLine("Пользователь успешно обновлен.");
            }
            else
            {
                Console.WriteLine("Ошибка при обновлении пользователя: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return result;
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }
    }
}
