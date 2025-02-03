namespace TestFullstack.Server.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public int? OrderNumber { get; set; }
        public string? Status { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
