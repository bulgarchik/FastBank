using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;

namespace FastBank.Services
{
    public class CustomerService : ICustomerService
    {
        readonly private ICustomerRepository _customerRepo;

        public CustomerService() 
        {
            _customerRepo = new CustomerRepository();
        }

        public List<Customer> GetAll()
        {
            return _customerRepo.GetAll();
        }

        public void Add(Customer customer)
        {
            _customerRepo.Add(customer);
        }

        public void Add(string name, string email, DateTime birthday, string password, Roles role)
        {
            var customer = new Customer(Guid.NewGuid(), name, email, birthday, password, role);

           Add(customer);
        }


    }
}
