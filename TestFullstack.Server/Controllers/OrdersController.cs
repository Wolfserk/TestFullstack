﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestFullstack.Server.Entities;
using TestFullstack.Server.Models;
using TestFullstack.Server.Services.Orders;
using TestFullstack.Server.Services.Users;

namespace TestFullstack.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrdersController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("myorders")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAllUserOrders()
        {
            var user = await _userService.GetUserAsync(User);

            if (user == null || user.CustomerId == null)
                return BadRequest("Пользователь не привязан к заказчику.");

            var orders = await _orderService.GetAllUserOrdersAsync((Guid)user.CustomerId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([FromBody] OrderRequestModel model)
        {
            if (model.Items == null || !model.Items.Any())
                return BadRequest("Заказ не может быть пустым.");

            var user = await _userService.GetUserAsync(User);
            if (user == null || user.CustomerId == null)
                return BadRequest("Пользователь не привязан к заказчику.");

            var orderItems = model.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                ItemId = i.ItemId,
                ItemsCount = i.Quantity,
                ItemPrice = i.Price
            }).ToList();

            var order = await _orderService.PlaceOrderAsync((Guid)user.CustomerId, orderItems);

            return Ok(new { message = "Заказ успешно создан!", orderId = order.Id });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound("Order not found");

            if (order.Status != "Новый")
                return BadRequest("Only new orders can be deleted");

            await _orderService.DeleteOrderAsync(id);
            return Ok("Order deleted");
        }
    }
}
