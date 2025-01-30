using TestFullstack.Server.DTOs;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Services.Orders
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<OrderDTO>> GetAllUserOrdersAsync(Guid customerId);
        Task<OrderDTO> GetOrderByIdAsync(Guid id);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Guid id);
        Task<Order> PlaceOrderAsync(Guid customerId, List<OrderItem> items);
    }
}
