namespace TestFullstack.Server.Models
{
    public class OrderRequestModel
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
