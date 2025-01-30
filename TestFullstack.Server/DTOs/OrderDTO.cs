namespace TestFullstack.Server.DTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public int? OrderNumber { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}
