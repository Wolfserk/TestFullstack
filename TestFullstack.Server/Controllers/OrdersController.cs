using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestFullstack.Server.Models;
using TestFullstack.Server.Services.Orders;

namespace TestFullstack.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
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
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _orderService.AddOrderAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }


        [HttpPost("{id}/confirm")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ConfirmOrder(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound("Order not found");

            order.Status = "Выполняется";
            order.ShipmentDate = DateTime.Now.AddDays(3); // Условная дата доставки
            await _orderService.UpdateOrderAsync(order);

            return Ok("Order confirmed");
        }

        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CompleteOrder(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound("Order not found");

            order.Status = "Выполнен";
            await _orderService.UpdateOrderAsync(order);

            return Ok("Order completed");
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


        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Order updatedOrder)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound("Заказ не найден");

            order.Status = updatedOrder.Status;
            order.ShipmentDate = updatedOrder.ShipmentDate;
            await _orderService.UpdateOrderAsync(order);

            return Ok(new { message = "Заказ обновлен", order });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound("Заказ не найден");

            if (order.Status != "Новый")
                return BadRequest("Удалять можно только заказы со статусом 'Новый'");

            await _orderService.DeleteOrderAsync(id);
            return Ok(new { message = "Заказ удален" });
        }
    }
}
