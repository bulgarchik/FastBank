using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;

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
            return _repo.Set<BankAccountDTO>()
                .Where(a => a.UserId == customer.Id)
                .Include(a => a.User)
                .Select(a => a.ToDomainObj())
                .FirstOrDefault();
        }

        public void Update(BankAccount bankAccount)
        {
            var bankAccountDto = _repo.Set<BankAccountDTO>()
                                      .Where(a => a.BankAccountId == bankAccount.BankAccountId)
                                      .FirstOrDefault();
            if (bankAccountDto != null) 
            {
                bankAccountDto.Amount = bankAccount.Amount;
                _repo.Update<BankAccountDTO>(bankAccountDto);
            }
        }
    }
}
