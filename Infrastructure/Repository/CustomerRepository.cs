using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using Infrastructure.Context;

namespace FastBank.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly FastBankDbContext _db;

        public CustomerRepository()
        {
           _db = new FastBankDbContext();
           _db.Database.EnsureCreated();
        }

        public List<Customer> GetAll()
        {
            var repo = new FastBank.Repository(_db);
            var customers = repo.Set<CustomerDTO>().Select(a=> a.ToDomainObj()).ToList();
            
            return customers;
        }
    }
}
