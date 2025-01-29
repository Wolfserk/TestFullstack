using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TestFullstack.Server.Models;
using TestFullstack.Server.Services.Users;

namespace TestFullstack.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(IUserService userService, RoleManager<IdentityRole> roleManager)
        {
            _userService = userService;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userService.GetRolesAsync(user);
                userList.Add(new
                {
                    user.Id,
                    user.Email,
                    Role = roles[0]=="Manager"?"Менеджер":"Заказчик"
                });
            }

            return Ok(userList);
        }

        [HttpPost("{id}/add-to-role")]
        public async Task<IActionResult> AddToRole(string id, [FromBody] string role)
        {
            var result = await _userService.AddToRoleAsync(id, role);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Роль добавлена пользователю");
        }

        // Удалить пользователя из роли
        [HttpPost("{id}/remove-from-role")]
        public async Task<IActionResult> RemoveFromRole(string id, [FromBody] string role)
        {
            var result = await _userService.RemoveFromRoleAsync(id, role);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"Пользователь с ID {id} был удален из роли {role}");
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Проверяем, существует ли указанная роль
            if (!await _roleManager.RoleExistsAsync(model.Role))
                return BadRequest($"Роль '{model.Role}' не существует");

           
            // Вызываем метод UserService
            var result = await _userService.AddUserAsync(model);

            // Обрабатываем результат
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors); 
            }
               

            return Ok("Пользователь успешно добавлен");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"Пользователь с ID {id} был удален");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string Id, [FromBody] UpdateUserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Проверяем, существует ли новая роль
            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                return BadRequest("Роли не существует");
            }
            var result = await _userService.UpdateUserAsync(Id, model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Пользователь успешно обновлен");
        }
    }
}
