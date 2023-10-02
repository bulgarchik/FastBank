using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using FastBank.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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
            var customers = _repo.SetNoTracking<CustomerDTO>().Select(a => a.ToDomainObj()).ToList();

            return customers;
        }

        //TODO Add method to get customer by email

        public void Add(Customer customer)
        {
            var customerDTO = new CustomerDTO(customer);
            _repo.Add(customerDTO);
        }
    }
}
