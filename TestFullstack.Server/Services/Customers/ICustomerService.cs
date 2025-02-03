using TestFullstack.Server.DTOs;
using TestFullstack.Server.Models;

namespace TestFullstack.Server.Services.Customers
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync();
        //Task<Customer> GetCustomerByCodeAsync(string code);
        Task<Customer> AddCustomerAsync(CreateCustomerDto dto);
        Task<Customer?> UpdateCustomerAsync(UpdateCustomerDto dto);
        Task AddCustomerToUserAsync(Guid? customerId, ApplicationUser user);
        Task<int> GetCustomerDiscountAsync(Guid customerId);
        Task DeleteCustomerAsync(Guid? customerId);
    }
}
