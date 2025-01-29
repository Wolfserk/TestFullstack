using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using TestFullstack.Server.Data;
using TestFullstack.Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TestFullstack.Server.Services.Customers;
using TestFullstack.Server.Services.Items;
using TestFullstack.Server.Services.Orders;
using TestFullstack.Server.Services.Users;


namespace TestFullstack.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                //options.Password.RequireDigit = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();



            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();



            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:54305")
                       .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sG2f3v5x8y/B1A2C3D4E5F6G7H8I9J0K1L2M3N4O5P6Q7R8S9T0U1V2W3X4"))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Ошибка аутентификации: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine($"Токен успешно валидирован для пользователя: {context.Principal.Identity.Name}");
                        return Task.CompletedTask;
                    }
                };
            });
            Console.WriteLine(builder.Configuration["Jwt:Issuer"]);
            Console.WriteLine(builder.Configuration["Jwt:Audience"]);
            Console.WriteLine(builder.Configuration["Jwt:Key"]);

            var app = builder.Build();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseDefaultFiles();
            app.MapStaticAssets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();          
        }
    }
}
