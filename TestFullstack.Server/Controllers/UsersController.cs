using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Services.Users;

namespace TestFullstack.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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
                    Role = roles[0] == "Manager" ? "Менеджер" : "Заказчик"
                });
            }

            return Ok(userList);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _userService.RoleExistsAsync(model.Role)) return BadRequest($"Роль '{model.Role}' не существует");

            var result = await _userService.AddUserAsync(model);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Пользователь успешно добавлен");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok($"Пользователь с ID {id} был удален");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _userService.RoleExistsAsync(model.Role))
            {
                return BadRequest("Роли не существует");
            }
            var result = await _userService.UpdateUserAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Пользователь успешно обновлен");
        }
    }
}
