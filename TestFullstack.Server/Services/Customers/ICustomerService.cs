using TestFullstack.Server.Models;

namespace TestFullstack.Server.Services.Customers
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByCodeAsync(string code);
        Task AddCustomerAsync(Customer customer);
    }
}
