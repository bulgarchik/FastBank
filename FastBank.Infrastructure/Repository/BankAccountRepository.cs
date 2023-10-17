using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.DTOs;

namespace FastBank.Infrastructure.Repository
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly Repository _repo;
        public BankAccountRepository()
        {
            _repo = new Repository(new FastBankDbContext());
        }

        public void Add(BankAccount bankAccount)
        {
            _repo.Add<BankAccountDTO>(new BankAccountDTO(bankAccount));
        }

        public BankAccount? GetBankAccountByCustomer(Customer customer)
        {
            return _repo.SetNoTracking<BankAccountDTO>().Where(a => a.CustomerDTO == new CustomerDTO(customer)).Select(a => a.ToDomainObj()).FirstOrDefault();
        }

        public void Update(BankAccount bankAccount)
        {
            _repo.Update<BankAccountDTO>(new BankAccountDTO(bankAccount));
        }
    }
}
