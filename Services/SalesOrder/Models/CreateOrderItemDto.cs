using System.ComponentModel.DataAnnotations;

namespace SalesOrderAPI.Services;

public class CreateOrderItemDto
{
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal Price { get; set; }
}
