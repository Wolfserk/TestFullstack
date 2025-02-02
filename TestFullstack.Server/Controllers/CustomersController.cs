using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Models;
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

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var customer = await _customerService.GetCustomerByCodeAsync(code);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Некорректные данные.");
            }

            try
            {
                var user = await _userService.GetUserAsync(User);
                var customer = await _customerService.AddCustomerAsync(request.Name, request.Address, user.Id);
                return Ok(customer.Id);
                //return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerDto dto)
        {
            var customer = await _customerService.UpdateCustomerAsync(id, dto);
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
