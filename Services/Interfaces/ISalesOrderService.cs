using SalesOrderAPI.Models;

namespace SalesOrderAPI.Services;

public interface ISalesOrderService
{
    Task<SalesOrder?> CreateOrderAsync(CreateSalesOrderDto createOrderDto);
    Task<SalesOrder?> GetOrderAsync(int id);
    Task<List<SalesOrder>> GetOrdersAsync();
    Task<OrderItem?> UpdateOrderItemAsync(int orderId, Guid itemId, OrderItemDto updatedOrderItemDto);
    Task<bool> DeleteOrderAsync(int id);
    Task<bool> DeleteOrderItemAsync(int orderId, Guid itemId);
    Task<OrderItem?> AddOrderItemAsync(int orderId, CreateOrderItemDto newOrderItemDto);
}
