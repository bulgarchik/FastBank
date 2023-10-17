using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.DTOs;

namespace FastBank.Infrastructure.Repository
{
    internal class BankAccountRepository : IBankAccountRepository
    {
        private readonly Repository _repo;
        public BankAccountRepository()
        {
            _repo = new Repository(new FastBankDbContext());
        }

        public BankAccount? GetBankAccountByCustomer(Customer customer)
        {
            return _repo.SetNoTracking<BankAccountDTO>().Where(a => a.CustomerDTO == new CustomerDTO(customer)).Select(a => a.ToDomainObj()).FirstOrDefault();
        }
    }
}
