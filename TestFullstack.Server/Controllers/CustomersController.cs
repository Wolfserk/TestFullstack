﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Services.Customers;
using TestFullstack.Server.Services.Users;

namespace TestFullstack.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;

        public CustomersController(ICustomerService customerService, IUserService userService)
        {
            _customerService = customerService;
            _userService = userService;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Некорректные данные.");
            }

            try
            {
                var user = await _userService.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest("Пользователь не найден.");
                }
                var customer = await _customerService.AddCustomerAsync(request);
                await _customerService.AddCustomerToUserAsync(customer.Id, user);

                return Ok(customer.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerDto dto)
        {
            var customer = await _customerService.UpdateCustomerAsync(dto);
            if (customer == null) return NotFound("Заказчик не найден");
            return Ok(customer);
        }

        [HttpGet("discount")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCustomerDiscount()
        {
            var user = await _userService.GetUserAsync(User);

            if (user == null || user.CustomerId == null)
                return BadRequest("Пользователь не привязан к заказчику.");

            var discount = await _customerService.GetCustomerDiscountAsync((Guid)user.CustomerId);
            return Ok(new { discount });
        }
    }
}
