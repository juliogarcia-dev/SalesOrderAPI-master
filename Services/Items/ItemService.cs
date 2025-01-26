using SalesOrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SalesOrderAPI.Services;
public class ItemsService : IItemService
{
    private readonly AppDbContext _dbContext;

    public ItemsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Item> CreateItemAsync(Item item)
    {
        _dbContext.Items.Add(item);
        await _dbContext.SaveChangesAsync();
        return item;
    }

    public async Task<Item> GetItemAsync(int id)
    {
        var item = await _dbContext.Items
        .AsNoTracking()
        .FirstOrDefaultAsync(i => i.Id == id);

        return item ?? new Item();
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        return await _dbContext.Items
            .OrderBy(i => i.Name)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Item?> UpdateItemAsync(int id, Item item)
    {
        var existingItem = await _dbContext.Items
            .FirstOrDefaultAsync(i => i.Id == id);


        if (existingItem == null)
        {
            return null;
        }

        existingItem.Name = item.Name;
        existingItem.Price = item.Price;

        _dbContext.Items.Update(existingItem);
        await _dbContext.SaveChangesAsync();

        return existingItem;
    }

    public async Task DeleteItemAsync(int id)
    {
        var item = await _dbContext.Items
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item != null)
        {
            _dbContext.Items.Remove(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}