using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Data;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Repositories.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationContext _context;

        public CustomerRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByCodeAsync(string code)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> AddAsync(Customer customer)
        {     
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> UpdateAsync(Guid id, Customer customerData)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return null;



            customer.Name = customerData.Name;
            customer.Code = customerData.Code;
            customer.Address = customerData.Address;
            customer.Discount = customerData.Discount;

            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<int> GetDiscountAsync(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            return customer?.Discount ?? 0;
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}
