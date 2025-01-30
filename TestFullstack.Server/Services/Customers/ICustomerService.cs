using TestFullstack.Server.Models;

namespace TestFullstack.Server.Services.Customers
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByCodeAsync(string code);
        Task<Customer> AddCustomerAsync(string name, string address, string userId);


    }
}
