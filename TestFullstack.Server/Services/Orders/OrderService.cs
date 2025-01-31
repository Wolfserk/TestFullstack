using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Data;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationContext _context;

        public OrderService(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<List<OrderDto>> GetAllUserOrdersAsync(Guid customerId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                Status = o.Status,
                OrderNumber = o.OrderNumber,
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ItemId = oi.ItemId,
                    ItemName = oi.Item.Name,
                    ItemsCount = oi.ItemsCount,
                    ItemPrice = oi.ItemPrice
                }).ToList()
            }).ToList();
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                ShipmentDate = o.ShipmentDate,
                Status = o.Status,
                OrderNumber = o.OrderNumber,
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ItemId = oi.ItemId,
                    ItemName = oi.Item.Name,             
                    ItemsCount = oi.ItemsCount,
                    ItemPrice = oi.ItemPrice
                }).ToList()
            }).ToList();
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return null!;

            return new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ItemId = oi.ItemId,
                    ItemsCount = oi.ItemsCount,
                    ItemPrice = oi.ItemPrice
                }).ToList()
            };
        }

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Order> PlaceOrderAsync(Guid customerId, List<OrderItem> items)
        {
            if (items == null || !items.Any())
                throw new ArgumentException("Заказ не может быть пустым.");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                Status = "Новый",
                OrderItems = items
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }
        public async Task<bool> ConfirmOrderAsync(ConfirmOrderDto request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null || order.Status != "Новый") return false;

            order.Status = "Выполняется";
            order.OrderNumber = request.OrderNumber;
            order.ShipmentDate = request.ShipmentDate;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CompleteOrderAsync(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.Status != "Выполняется") return false;

            order.Status = "Выполнен";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
