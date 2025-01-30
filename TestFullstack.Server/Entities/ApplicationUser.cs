using Microsoft.AspNetCore.Identity;

namespace TestFullstack.Server.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
