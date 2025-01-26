namespace SalesOrderAPI.Services;

public class UpdateItemDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
}
