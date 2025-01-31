using TestFullstack.Server.DTOs;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Services.Items
{
    public interface IItemService
    {
        Task<List<Item>> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(Guid id);
        Task<Item> AddItemAsync(ItemDto itemDto);
        Task<Item> UpdateItemAsync(Guid id, ItemDto itemDto);
        Task<bool> DeleteItemAsync(Guid id);

        Task<List<ItemPriceDto>> GetItemPricesAsync(List<Guid> itemIds);
    }
}
