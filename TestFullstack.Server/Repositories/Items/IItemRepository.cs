using TestFullstack.Server.DTOs;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Repositories.Items
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAllItemsAsync();
        Task<Item?> GetItemByIdAsync(Guid id);
        Task<Item> AddItemAsync(Item item);
        Task<Item?> UpdateItemAsync(Item item);
        Task<bool> DeleteItemAsync(Guid id);
        Task<List<ItemPriceDto>> GetItemPricesAsync(List<Guid> itemIds);
    }
}
