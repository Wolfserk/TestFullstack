using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Data;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Models;
using TestFullstack.Server.Repositories.Orders;

namespace TestFullstack.Server.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return orders.Select(MapToOrderDto).ToList();
        }

        public async Task<List<OrderDto>> GetAllUserOrdersAsync(Guid customerId)
        {
            var orders = await _orderRepository.GetAllUserOrdersAsync(customerId);
            return orders.Select(MapToOrderDto).ToList();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            return order == null ? null : MapToOrderDto(order);
        }

        public async Task<Order> PlaceOrderAsync(Guid customerId, List<OrderItem> items)
        {
            if (!items.Any()) throw new ArgumentException("Заказ не может быть пустым.");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                Status = "Новый",
                OrderItems = items
            };

            return await _orderRepository.PlaceOrderAsync(order);
        }

        public async Task<bool> ConfirmOrderAsync(ConfirmOrderDto request)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order == null || order.Status != "Новый") return false;

            order.Status = "Выполняется";
            order.OrderNumber = request.OrderNumber;
            order.ShipmentDate = request.ShipmentDate;

            return await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task<bool> CompleteOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null || order.Status != "Выполняется") return false;

            order.Status = "Выполнен";
            return await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task<bool> DeleteOrderAsync(Guid id) 
        {
            return await _orderRepository.DeleteOrderAsync(id);
        }

        private static OrderDto MapToOrderDto(Order order) => new()
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            Status = order.Status!,
            OrderNumber = order.OrderNumber,
            ShipmentDate = order.ShipmentDate,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                ItemId = oi.ItemId,
                ItemName = oi.Item.Name,
                ItemsCount = oi.ItemsCount,
                ItemPrice = oi.ItemPrice
            }).ToList()
        };
    }
}
