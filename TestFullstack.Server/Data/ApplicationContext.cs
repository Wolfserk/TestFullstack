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

            //modelBuilder.Entity<Customer>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Code).IsRequired();
            //    entity.Property(e => e.Name).IsRequired();
            //});

            modelBuilder.Entity<Customer>()
                     .HasOne(c => c.User)
                     .WithOne(u => u.Customer)
                     .HasForeignKey<ApplicationUser>(u => u.CustomerId)
                     .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(o => o.Customer)
                      .WithMany(c => c.Orders)
                      .HasForeignKey(o => o.CustomerId);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(oi => oi.OrderId);
            });

            modelBuilder.Entity<OrderItem>()
           .HasOne(oi => oi.Item)
           .WithMany()
           .HasForeignKey(oi => oi.ItemId);

            modelBuilder.Entity<ApplicationUser>()
           .HasOne(u => u.Customer)
           .WithMany()
           .HasForeignKey(u => u.CustomerId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole {Id= "f1811537-a05b-49bb-bee9-7a9480267c12", Name = "Manager", NormalizedName = "MANAGER" },
               new IdentityRole { Id = "f67b8dc6-0bee-4732-85fc-ff31a90615ad", Name = "Customer", NormalizedName = "CUSTOMER" }
               );

        }
    }
}
