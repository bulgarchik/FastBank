using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using FastBank.Infrastructure.Context;

namespace FastBank.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Repository _repo;

        public CustomerRepository()
        {
            _repo = new Repository(new FastBankDbContext());
        }

        public List<Customer> GetAll()
        {
            var customers = _repo.SetNoTracking<CustomerDTO>()
                                 .Select(a => a.ToDomainObj())
                                 .ToList();

            return customers;
        }

        public Customer? GetByEmail(string email) 
        {
            var customer = _repo.SetNoTracking<CustomerDTO>()
                                .Where(c => c.Email == email)
                                .Select(a => a.ToDomainObj())
                                .ToList()
                                .FirstOrDefault();

            return customer;
        }

        public void Add(Customer customer)
        {
            var customerDTO = new CustomerDTO(customer);
            _repo.Add(customerDTO);
        }
    }
}
