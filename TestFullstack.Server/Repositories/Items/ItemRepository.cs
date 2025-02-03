using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Data;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Models;

namespace TestFullstack.Server.Repositories.Items
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationContext _context;

        public ItemRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetAllItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item?> GetItemByIdAsync(Guid id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<Item> AddItemAsync(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item?> UpdateItemAsync(Item item)
        {
            var existingItem = await _context.Items.FindAsync(item.Id);
            if (existingItem == null) return null;

            existingItem.Code = item.Code;
            existingItem.Name = item.Name;
            existingItem.Price = item.Price;
            existingItem.Category = item.Category;

            await _context.SaveChangesAsync();
            return existingItem;
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
                .Select(i => new ItemPriceDto { Id = i.Id, Price = i.Price })
                .ToListAsync();
        }
    }
}
