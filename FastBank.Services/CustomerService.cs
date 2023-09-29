using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;

namespace FastBank.Services
{
    public class CustomerService : ICustomerService
    {
        public List<Customer> GetAll()
        {
            ICustomerRepository rp = new CustomerRepository();

            return rp.GetAll();
        }
    }
}
