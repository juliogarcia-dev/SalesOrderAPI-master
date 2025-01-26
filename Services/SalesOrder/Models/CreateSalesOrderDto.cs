using System.ComponentModel.DataAnnotations;

namespace SalesOrderAPI.Services;

public class CreateSalesOrderDto
{
    [Required]
    public List<CreateOrderItemDto> Items { get; set; } = [];
}
