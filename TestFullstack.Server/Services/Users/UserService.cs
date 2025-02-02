using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TestFullstack.Server.Entities;
using TestFullstack.Server.Services.Customers;

namespace TestFullstack.Server.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomerService _customerService;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ICustomerService customerService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _customerService = customerService;
        }

        public async Task<bool> RoleExistsAsync(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }
        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<IdentityResult> AddUserAsync(RegisterModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            await _userManager.CreateAsync(user, model.Password);
            return await AddToRoleAsync(user.Id, model.Role);
        }

        public async Task<IdentityResult> AddToRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed();
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed();

            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user.CustomerId != null) await _customerService.DeleteCustomerAsync(user.CustomerId);
            return user == null ? IdentityResult.Failed() : await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(string Id, UpdateUserModel model)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = $"Пользователь с Id '{Id}' не найден"
                });
            }

            if (!string.IsNullOrEmpty(model.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);

                if(user.CustomerId!=null)
                {
                   await _customerService.DeleteCustomerAsync(user.CustomerId);
                }
 
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                
                if (!removeRolesResult.Succeeded)
                {
                    return removeRolesResult;
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, model.Role);
                if (!addRoleResult.Succeeded)
                {
                    return addRoleResult;
                }
            }

            if (!string.IsNullOrEmpty(model.Email) && user.Email != model.Email)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return updateResult;
                }
            }

            return IdentityResult.Success;
        }
    }
}
