namespace TestFullstack.Server.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public int? OrderNumber { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
