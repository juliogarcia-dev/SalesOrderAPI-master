using System.ComponentModel.DataAnnotations;

namespace SalesOrderAPI.Services;

public class OrderItemDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal Price { get; set; }
}
