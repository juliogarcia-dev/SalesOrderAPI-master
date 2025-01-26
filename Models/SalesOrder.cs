namespace SalesOrderAPI.Models
{
    public class SalesOrder
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public List<OrderItem> Items { get; set; } = [];
    }
}

