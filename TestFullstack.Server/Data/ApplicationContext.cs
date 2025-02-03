using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestFullstack.Server.Models;

namespace TestFullstack.Server.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
          : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
             .HasOne(c => c.User)
             .WithOne(u => u.Customer)
             .HasForeignKey<ApplicationUser>(u => u.CustomerId)
             .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Item)
            .WithMany(i => i.OrderItems)
            .HasForeignKey(oi => oi.ItemId)
            .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
