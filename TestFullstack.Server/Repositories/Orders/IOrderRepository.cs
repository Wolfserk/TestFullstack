using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<Order>> GetAllUserOrdersAsync(Guid customerId);
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<Order> PlaceOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(Guid id);
    }
}
