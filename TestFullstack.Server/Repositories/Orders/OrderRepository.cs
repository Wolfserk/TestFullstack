using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Data;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationContext _context;

        public OrderRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllUserOrdersAsync(Guid customerId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            //return await _context.Orders
            //    .Include(o => o.OrderItems)
            //    .FirstOrDefaultAsync(o => o.Id == id);
            return await _context.Orders
       .Include(o => o.OrderItems)
       .ThenInclude(oi => oi.Item) // Включаем связанные данные
       .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> PlaceOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
