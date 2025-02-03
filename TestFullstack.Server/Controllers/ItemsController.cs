using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Services.Items;

namespace TestFullstack.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Получить список всех товаров
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        /// <summary>
        /// Получить товар по ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(Guid id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
                return NotFound("Товар не найден");

            return Ok(item);
        }

        /// <summary>
        /// Добавить новый товар (только для менеджеров)
        /// </summary>
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _itemService.AddItemAsync(itemDto);
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }

        /// <summary>
        /// Обновить существующий товар (только для менеджеров)
        /// </summary>
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedItem = await _itemService.UpdateItemAsync(id, itemDto);
            if (updatedItem == null)
                return NotFound("Товар не найден");

            return Ok(updatedItem);
        }

        /// <summary>
        /// Удалить товар (только для менеджеров)
        /// </summary>
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var result = await _itemService.DeleteItemAsync(id);
            if (!result)
                return NotFound("Товар не найден");

            return Ok(new { message = "Товар успешно удален" });
        }

        /// <summary>
        /// Получить цены товаров по списку ID
        /// </summary>
        [HttpPost("get-prices")]
        public async Task<IActionResult> GetItemPrices([FromBody] ItemPriceRequestDto request)
        {
            if (request.ItemIds == null || !request.ItemIds.Any())
                return BadRequest("Список товаров пуст");

            var prices = await _itemService.GetItemPricesAsync(request.ItemIds);
            return Ok(prices);
        }
    }
}
