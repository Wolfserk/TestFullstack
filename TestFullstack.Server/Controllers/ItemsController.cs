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

        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            return Ok(await _itemService.GetAllItemsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(Guid id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null) return NotFound("Товар не найден");

            return Ok(item);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ItemDTO itemDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var item = await _itemService.AddItemAsync(itemDto);
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] ItemDTO itemDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedItem = await _itemService.UpdateItemAsync(id, itemDto);
            if (updatedItem == null) return NotFound("Товар не найден");

            return Ok(updatedItem);
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var result = await _itemService.DeleteItemAsync(id);
            if (!result) return NotFound("Товар не найден");

            return Ok("Товар успешно удален");
        }
    }
}
