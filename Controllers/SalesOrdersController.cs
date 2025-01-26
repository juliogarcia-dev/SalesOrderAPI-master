using Microsoft.AspNetCore.Mvc;
using SalesOrderAPI.Services;

namespace SalesOrderAPI.Controllers;

[Route("SalesOrderAPI/[controller]")]
[ApiController]
public class SalesOrdersController : ControllerBase
{
    private readonly ISalesOrderService _salesOrderService;
    public SalesOrdersController(ISalesOrderService salesOrderService)
    {
        _salesOrderService = salesOrderService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateSalesOrderDto createOrderDto)
    {
        if (createOrderDto == null || createOrderDto.Items == null || !createOrderDto.Items.Any())
        {
            return BadRequest("Order and OrderItems cannot be null or empty.");
        }
        var createdOrder = await _salesOrderService.CreateOrderAsync(createOrderDto);
        if (createdOrder == null)
        {
            return StatusCode(500, "Failed to create the order.");
        }
        return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _salesOrderService.GetOrderAsync(id);
        if (order == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }
        return Ok(order);
    }
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _salesOrderService.GetOrdersAsync();
        return Ok(orders);
    }
    [HttpPut("{id}/OrderItem/{itemId}")]
    public async Task<IActionResult> UpdateOrderItem(int id, Guid itemId, [FromBody] OrderItemDto updatedOrderItemDto)
    {
        var updatedItem = await _salesOrderService.UpdateOrderItemAsync(id, itemId, updatedOrderItemDto);
        if (updatedItem == null)
        {
            return NotFound($"OrderItem with ID {itemId} not found in Order {id}.");
        }
        return Ok(updatedItem);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var result = await _salesOrderService.DeleteOrderAsync(id);
        if (!result)
        {
            return NotFound($"Order with ID {id} not found.");
        }
        return NoContent();
    }
    [HttpDelete("{id}/OrderItem/{itemId}")]
    public async Task<IActionResult> DeleteOrderItem(int id, Guid itemId)
    {
        var result = await _salesOrderService.DeleteOrderItemAsync(id, itemId);
        if (!result)
        {
            return NotFound($"OrderItem with ID {itemId} not found in Order {id}.");
        }
        return NoContent();
    }
    [HttpPost("{id}/OrderItem")]
    public async Task<IActionResult> AddOrderItem(int id, [FromBody] CreateOrderItemDto newOrderItemDto)
    {
        if (newOrderItemDto == null || newOrderItemDto.Quantity <= 0 || newOrderItemDto.Price <= 0)
        {
            return BadRequest("Invalid OrderItem data.");
        }
        var addedItem = await _salesOrderService.AddOrderItemAsync(id, newOrderItemDto);
        if (addedItem == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }
        return CreatedAtAction(nameof(GetOrder), new { id }, addedItem);
    }
}