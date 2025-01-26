using SalesOrderAPI.Models;

namespace SalesOrderAPI.Services;

public interface IItemService
{
    Task<Item> CreateItemAsync(Item item);
    Task<Item> GetItemAsync(int id);
    Task<List<Item>> GetItemsAsync();
    Task<Item?> UpdateItemAsync(int id, Item item);
    Task DeleteItemAsync(int id);
}
