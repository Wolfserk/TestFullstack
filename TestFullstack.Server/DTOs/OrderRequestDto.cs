namespace TestFullstack.Server.DTOs
{
    public class OrderRequestDto
    {
        public List<OrderItemRequest> Items { get; set; }
    }

    public class OrderItemRequest
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
