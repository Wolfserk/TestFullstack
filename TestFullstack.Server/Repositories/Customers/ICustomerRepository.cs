using TestFullstack.Server.Entities;

namespace TestFullstack.Server.Repositories.Customers
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByCodeAsync(string code);
        Task<Customer?> GetByIdAsync(Guid id);
        Task<Customer> AddAsync(Customer customer);
        Task<Customer?> UpdateAsync(Guid id, Customer customer);
        Task<int> GetDiscountAsync(Guid customerId);
        Task DeleteAsync(Guid id);
    }
}
