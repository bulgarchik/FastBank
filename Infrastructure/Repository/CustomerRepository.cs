using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using Infrastructure.Context;

namespace FastBank.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly FastBank.Repository _repo;

        public CustomerRepository()
        {
            _repo = new FastBank.Repository(new FastBankDbContext());
        }

        public List<Customer> GetAll()
        {
            var customers = _repo.Set<CustomerDTO>().Select(a => a.ToDomainObj()).ToList();

            return customers;
        }

        public void Add(Customer customer)
        {
            var customerDTO = new CustomerDTO(customer);
            _repo.Add(customerDTO);
        }
    }
}
