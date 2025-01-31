using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Data;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Services.Items
{
    public class ItemService : IItemService
    {
        private readonly ApplicationContext _context;

        public ItemService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetAllItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> GetItemByIdAsync(Guid id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<Item> AddItemAsync(ItemDto itemDto)
        {
            var item = new Item
            {
                Code = itemDto.Code,
                Name = itemDto.Name,
                Price = itemDto.Price,
                Category = itemDto.Category
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item> UpdateItemAsync(Guid id, ItemDto itemDto)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return null;

            item.Code = itemDto.Code;
            item.Name = itemDto.Name;
            item.Price = itemDto.Price;
            item.Category = itemDto.Category;

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return false;

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ItemPriceDto>> GetItemPricesAsync(List<Guid> itemIds)
        {
            return await _context.Items
                .Where(i => itemIds.Contains(i.Id))
                .Select(i => new ItemPriceDto
                {
                    Id = i.Id,
                    Price = i.Price
                })
                .ToListAsync();
        }
    }
}
