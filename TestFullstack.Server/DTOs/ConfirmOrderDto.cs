namespace TestFullstack.Server.DTOs
{
    public class ConfirmOrderDto
    {
        public Guid OrderId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime ShipmentDate { get; set; }
    }
}
