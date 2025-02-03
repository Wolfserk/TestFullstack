using TestFullstack.Server.DTOs;
using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Services.Customers
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByCodeAsync(string code);
        Task<Customer> AddCustomerAsync(string name, string? address, string userId);
        Task<Customer?> UpdateCustomerAsync(Guid id, UpdateCustomerDto dto);
        Task AddCustomerToUserAsync(Guid? customerId, ApplicationUser user);
        Task<int> GetCustomerDiscountAsync(Guid customerId);
        Task DeleteCustomerAsync(Guid? customerId);
    }
}
