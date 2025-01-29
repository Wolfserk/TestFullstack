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
            //var customers = await _customerService.GetAllCustomersAsync();
            var customers = new List<Customer>();
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Test13",
                Code = "wawgawgawg",
                Address = "awgwa"
            };
            var customer2 = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Test2",
                Code = "wawgawgawg",
                Address = "awgwa"
            };
            customers.Add(customer);
            customers.Add(customer2);
            customers.Add(customer);
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

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] Customer customer)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _customerService.AddCustomerAsync(customer);
                return CreatedAtAction(nameof(GetByCode), new { code = customer.Code }, customer);
            }
        }
}
