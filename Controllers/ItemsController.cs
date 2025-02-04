using Microsoft.AspNetCore.Mvc;
using SalesOrderAPI.Models;
using SalesOrderAPI.Services;

namespace SalesOrderAPI.Controllers;

[Route("SalesOrderAPI/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;
    public ItemsController(IItemService itemService)
    {
        _itemService = itemService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Invalid item data.");
        }
        var createdItem = await _itemService.CreateItemAsync(new Item
        {
            Name = dto.Name,
            Price = dto.Price
        });
        return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, createdItem);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(int id)
    {
        var item = await _itemService.GetItemAsync(id);
        if (item == null)
        {
            return NotFound($"Item with ID {id} not found.");
        }
        return Ok(item);
    }
    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        var items = await _itemService.GetItemsAsync();
        return Ok(items);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, [FromBody] UpdateItemDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("Item ID mismatch.");
        }
        var updatedItem = await _itemService.UpdateItemAsync(id, new Item
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price
        });
        if (updatedItem == null)
        {
            return NotFound($"Item with ID {id} not found.");
        }
        return Ok(updatedItem);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        await _itemService.DeleteItemAsync(id);         
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetNameItems([FromQuery] string name)
    {
        var items = await _itemService.GetNameItemsAsync(name);
        return Ok(items);
    }
}
