namespace SalesOrderAPI.Models
{    
    public class OrderItem
    {
        public Guid Id { get; set; }
        public int SalesOrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}