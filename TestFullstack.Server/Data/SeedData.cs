using Microsoft.AspNetCore.Identity;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<IdentityRole>
            {
                new() { Id = "f1811537-a05b-49bb-bee9-7a9480267c12", Name = "Manager", NormalizedName = "MANAGER" },
                new() { Id = "f67b8dc6-0bee-4732-85fc-ff31a90615ad", Name = "Customer", NormalizedName = "CUSTOMER" }
            };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            if (!context.Items.Any())
            {
                var items = new List<Item>();

                for (int i = 1; i < 7; i++)
                {
                    items.Add(new Item { 
                        Name = "Item"+i, 
                        Code = "00-0000-AA0"+i, 
                        Price = new Random().Next(1000, 3000), 
                        Category = "Category"+i 
                    });
                }
             
                context.Items.AddRange(items);
                context.SaveChanges();
            }
        }
    }
}
