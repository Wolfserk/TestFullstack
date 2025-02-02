using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Data;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Entities;
using TestFullstack.Server.Repositories.Customers;

namespace TestFullstack.Server.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer?> GetCustomerByCodeAsync(string code)
        {
            return await _customerRepository.GetByCodeAsync(code);
        }

        public async Task<Customer> AddCustomerAsync(string name, string? address, string userId)
        {
            var year = DateTime.UtcNow.Year;
            var lastCustomer = (await _customerRepository.GetAllAsync())
                .Where(c => c.Code.StartsWith(year.ToString()))
                .OrderByDescending(c => c.Code)
                .FirstOrDefault();

            int lastId = 1;
            if (lastCustomer != null)
            {
                var parts = lastCustomer.Code.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[0], out int parsedId))
                {
                    lastId = parsedId + 1;
                }
            }

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = name,
                Address = address,
                Discount = 0,
                Code = $"{lastId:D4}-{year}",
            };

            return await _customerRepository.AddAsync(customer);
        }

        public async Task<Customer?> UpdateCustomerAsync(Guid id, UpdateCustomerDto dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Code = dto.Code,
                Address = dto.Address,
                Discount = dto.Discount
            };
            return await _customerRepository.UpdateAsync(id, customer);
        }

        public async Task<int> GetCustomerDiscountAsync(Guid customerId)
        {
            return await _customerRepository.GetDiscountAsync(customerId);
        }

        public async Task DeleteCustomerAsync(Guid? customerId)
        {
            if (customerId == null) return;
            await _customerRepository.DeleteAsync(customerId.Value);
        }
    }
}
