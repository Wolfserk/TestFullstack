using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestFullstack.Server.Models;
using TestFullstack.Server.Services.Customers;

namespace TestFullstack.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

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
            if (request == null || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest("Некорректные данные.");
            }

            try
            {
                var customer = await _customerService.AddCustomerAsync(request.Name, request.Address, request.UserId);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }
    }
}
