using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Data;
using TestFullstack.Server.DTOs;
using TestFullstack.Server.Entities;
using TestFullstack.Server.Repositories.Items;

namespace TestFullstack.Server.Services.Items
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public Task<List<Item>> GetAllItemsAsync()
        {
            return _itemRepository.GetAllItemsAsync();
        }

        public Task<Item?> GetItemByIdAsync(Guid id)
        {
            return _itemRepository.GetItemByIdAsync(id);
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

            return await _itemRepository.AddItemAsync(item);
        }

        public async Task<Item?> UpdateItemAsync(Guid id, ItemDto itemDto)
        {
            var existingItem = await _itemRepository.GetItemByIdAsync(id);
            if (existingItem == null) return null;

            existingItem.Code = itemDto.Code;
            existingItem.Name = itemDto.Name;
            existingItem.Price = itemDto.Price;
            existingItem.Category = itemDto.Category;

            return await _itemRepository.UpdateItemAsync(existingItem);
        }

        public Task<bool> DeleteItemAsync(Guid id)
        {
            return _itemRepository.DeleteItemAsync(id);
        }

        public Task<List<ItemPriceDto>> GetItemPricesAsync(List<Guid> itemIds)
        {
            return _itemRepository.GetItemPricesAsync(itemIds);
        }
    }
}
