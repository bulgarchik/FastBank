using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Infrastructure.Repository
{
    public class BankRepository : IBankRepository
    {
        private readonly Repository _repo;
        public BankRepository()
        {
            _repo = new Repository(new FastBankDbContext());
        }

        public void ReplenishCapital(User user, Bank bank, decimal amountToReplenish)
        {
            var bankDto = _repo.Set<BankDTO>().Where(b => b.BankId == bank.Id).FirstOrDefault();

            if (bankDto != null)
            {
                bankDto.ReplenishCapital(amountToReplenish);

                _repo.Update<BankDTO>(bankDto);
            }
        }

        public Bank? GetBank()
        {
            return _repo.Set<BankDTO>().Select(a => a.ToDomainObj()).FirstOrDefault();
        }
    }
}
