using SalesOrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SalesOrderAPI.Services;

public class SalesOrdersService : ISalesOrderService
{
    private readonly AppDbContext _context;
    public SalesOrdersService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<SalesOrder?> CreateOrderAsync(CreateSalesOrderDto createOrderDto)
    {
        var salesOrder = new SalesOrder
        {
            OrderDate = DateTime.UtcNow,
            Total = createOrderDto.Items.Sum(i => i.Price * i.Quantity),
            Items = [.. createOrderDto.Items.Select(i => new OrderItem
            {
                Quantity = i.Quantity,
                Price = i.Price
            })]
        };
        _context.SalesOrders.Add(salesOrder);
        await _context.SaveChangesAsync();
        return salesOrder;
    }
    public async Task<SalesOrder?> GetOrderAsync(int id)
    {
        var salesOrder = await _context.SalesOrders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
        return salesOrder;
    }
    public async Task<List<SalesOrder>> GetOrdersAsync()
    {
        var salesOrders = await _context.SalesOrders
            .Include(o => o.Items)
            .ToListAsync();
        return salesOrders;
    }
    public async Task<OrderItem?> UpdateOrderItemAsync(int orderId, Guid itemId, OrderItemDto updatedItemDto)
    {
        var orderItem = await _context.OrderItems
            .FirstOrDefaultAsync(i => i.Id == itemId && i.SalesOrderId == orderId);
        if (orderItem == null)
            return null;
        orderItem.Quantity = updatedItemDto.Quantity;
        orderItem.Price = updatedItemDto.Price;
        _context.OrderItems.Update(orderItem);
        await _context.SaveChangesAsync();
        return orderItem;
    }
    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await _context.SalesOrders
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
            return false;
        _context.SalesOrders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteOrderItemAsync(int orderId, Guid itemId)
    {
        var orderItem = await _context.OrderItems
            .FirstOrDefaultAsync(i => i.Id == itemId && i.SalesOrderId == orderId);
        if (orderItem == null)
            return false;
        _context.OrderItems.Remove(orderItem);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<OrderItem?> AddOrderItemAsync(int orderId, CreateOrderItemDto newItemDto)
    {
        var salesOrder = await _context.SalesOrders
            .FirstOrDefaultAsync(o => o.Id == orderId);
        if (salesOrder == null)
            return null;
        var newItem = new OrderItem
        {
            Quantity = newItemDto.Quantity,
            Price = newItemDto.Price,
            SalesOrderId = orderId
        };
        salesOrder.Items.Add(newItem);
        await _context.SaveChangesAsync();
        return newItem;
    }
}