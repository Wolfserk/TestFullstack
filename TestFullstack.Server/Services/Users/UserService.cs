﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TestFullstack.Server.Entities;
using TestFullstack.Server.Services.Customers;
using TestFullstack.Server.Repositories.Users;

namespace TestFullstack.Server.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomerService _customerService;

        public UserService(IUserRepository userRepository, RoleManager<IdentityRole> roleManager, ICustomerService customerService)
        {
            _userRepository = userRepository;
            _roleManager = roleManager;
            _customerService = customerService;
        }

        public async Task<bool> RoleExistsAsync(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }

        public async Task<ApplicationUser?> GetUserAsync(ClaimsPrincipal user) // ✅ Используем метод репозитория
        {
            return await _userRepository.GetUserAsync(user);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userRepository.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AddUserAsync(RegisterModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userRepository.CreateUserAsync(user, model.Password);
            if (!result.Succeeded) return result;

            return await _userRepository.AddToRoleAsync(user, model.Role);
        }

        public async Task<IdentityResult> AddToRoleAsync(string userId, string role)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user == null ? IdentityResult.Failed() : await _userRepository.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(string userId, string role)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user == null ? IdentityResult.Failed() : await _userRepository.RemoveFromRoleAsync(user, role);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user?.CustomerId != null)
                await _customerService.DeleteCustomerAsync(user.CustomerId);

            return user == null ? IdentityResult.Failed() : await _userRepository.DeleteUserAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(string Id, UpdateUserModel model)
        {
            var user = await _userRepository.GetUserByIdAsync(Id);
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
                var currentRoles = await _userRepository.GetRolesAsync(user);
                if (user.CustomerId != null)
                {
                    await _customerService.DeleteCustomerAsync(user.CustomerId);
                }

                var removeRolesResult = await _userRepository.RemoveFromRoleAsync(user, currentRoles.First());
                if (!removeRolesResult.Succeeded)
                {
                    return removeRolesResult;
                }

                var addRoleResult = await _userRepository.AddToRoleAsync(user, model.Role);
                if (!addRoleResult.Succeeded)
                {
                    return addRoleResult;
                }
            }

            if (!string.IsNullOrEmpty(model.Email) && user.Email != model.Email)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                var updateResult = await _userRepository.UpdateUserAsync(user);
                if (!updateResult.Succeeded)
                {
                    return updateResult;
                }
            }

            return IdentityResult.Success;
        }
    }
}
