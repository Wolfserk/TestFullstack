using System.ComponentModel.DataAnnotations;

namespace TestFullstack.Server.DTOs
{
    public class AddUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
