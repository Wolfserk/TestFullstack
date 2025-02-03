using System.ComponentModel.DataAnnotations;

namespace TestFullstack.Server.Models
{
    public class Item
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [RegularExpression(@"^\d{2}-\d{4}-[A-Z]{2}\d{2}$", ErrorMessage = "Код товара должен быть в формате XX-XXXX-YYXX")]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        public decimal Price { get; set; }

        [MaxLength(30, ErrorMessage = "Название категории не должно превышать 30 символов")]
        public string Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
