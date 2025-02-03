namespace TestFullstack.Server.DTOs
{
    public class UpdateCustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public int Discount { get; set; }
    }
}
