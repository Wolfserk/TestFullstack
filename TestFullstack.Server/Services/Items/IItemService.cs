using TestFullstack.Server.DTOs;
using TestFullstack.Server.Models;

namespace TestFullstack.Server.Services.Items
{
    public interface IItemService
    {
        Task<List<Item>> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(Guid id);
        Task<Item> AddItemAsync(ItemDTO itemDto);
        Task<Item> UpdateItemAsync(Guid id, ItemDTO itemDto);
        Task<bool> DeleteItemAsync(Guid id);
    }
}
