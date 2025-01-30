using System.ComponentModel.DataAnnotations;

namespace TestFullstack.Server.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "Формат должен быть XXXX-ГГГГ")]
        public string Code { get; set; }
        public string? Address { get; set; }
        public int? Discount { get; set; }
        public ICollection<Order> Orders { get; set; }

        public ApplicationUser User { get; set; }
    }
}
