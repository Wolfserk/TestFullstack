using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Data;
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

        //public async Task<Customer> AddCustomerAsync(string name, string? address, string userId)
        //{
        //    var year = DateTime.UtcNow.Year;
        //    var lastId = _context.Customers.Count() + 1;

        //    var customer = new Customer
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = name,
        //        Address = address,
        //        Discount = 0,
        //        Code = $"{lastId:D4}-{year}",
        //    };

        //    _context.Customers.Add(customer);
        //    await _context.SaveChangesAsync();

        //    var user = await _context.Users.FindAsync(userId);
        //    if (user != null)
        //    {
        //        user.CustomerId = customer.Id;
        //        await _context.SaveChangesAsync();
        //    }

        //    return customer;
        //}

        public async Task<Customer> AddCustomerAsync(string name, string? address, string userId)
        {
            var year = DateTime.UtcNow.Year;

            // Получаем максимальный Code из существующих записей
            var maxCode = await _context.Customers
                .Where(c => c.Code.StartsWith(year.ToString())) // Фильтруем по году
                .OrderByDescending(c => c.Code)
                .Select(c => c.Code)
                .FirstOrDefaultAsync();

            int lastId = 1; // Начальное значение, если записей нет
            if (!string.IsNullOrEmpty(maxCode))
            {
                // Извлекаем числовую часть из Code (например, "0001-2023" -> 1)
                var parts = maxCode.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[0], out int parsedId))
                {
                    lastId = parsedId + 1; // Инкрементируем
                }
            }

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = name,
                Address = address,
                Discount = 0,
                Code = $"{lastId:D4}-{year}", // Форматируем Code
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
    }
}
