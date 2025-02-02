using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Data;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationContext _context;

        public CustomerService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerByCodeAsync(string code)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Code == code);
        }
        public async Task<Customer> AddCustomerAsync(string name, string? address, string userId)
        {
            var year = DateTime.UtcNow.Year;

            var maxCode = await _context.Customers
                .Where(c => c.Code.StartsWith(year.ToString()))
                .OrderByDescending(c => c.Code)
                .Select(c => c.Code)
                .FirstOrDefaultAsync();

            int lastId = 1;
            if (!string.IsNullOrEmpty(maxCode))
            {
                var parts = maxCode.Split('-');
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

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.CustomerId = customer.Id;
                await _context.SaveChangesAsync();
            }

            return customer;
        }
        public async Task<Customer?> UpdateCustomerAsync(Guid id, UpdateCustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return null;

            customer.Name = dto.Name;
            customer.Code = dto.Code;
            customer.Address = dto.Address;
            customer.Discount = dto.Discount;

            await _context.SaveChangesAsync();
            return customer;
        }
        public async Task<int> GetCustomerDiscountAsync(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            return customer?.Discount ?? 0;
        }

        public async Task DeleteCustomerAsync(Guid? id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.CustomerId == id);
            if (user != null)
            {
                user.CustomerId = null;
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

    }
}
