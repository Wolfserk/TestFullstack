﻿using TestFullstack.Server.DTOs;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Services.Orders
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<List<OrderDto>> GetAllUserOrdersAsync(Guid customerId);
        Task<OrderDto> GetOrderByIdAsync(Guid id);
        //Task AddOrderAsync(Order order);
        //Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Guid id);
        Task<Order> PlaceOrderAsync(Guid customerId, List<OrderItem> items);
        Task<bool> ConfirmOrderAsync(ConfirmOrderDto confirm);
        Task<bool> CompleteOrderAsync(Guid orderId);
    }
}
