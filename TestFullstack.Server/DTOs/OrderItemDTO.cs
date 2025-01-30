namespace TestFullstack.Server.DTOs
{
    public class OrderItemDTO
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemsCount { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
