using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.Repository;

namespace FastBank.Services.BankService
{
    public class BankService : IBankService
    {
        readonly private IRepository _repo;
        public BankService()
        {
            _repo = new Repository(new FastBankDbContext());
        }

        public void Add(Bank bank)
        {
            if (!_repo.Set<BankDTO>().Any())
            {
                var bankDTO = new BankDTO(10000m);
                _repo.Add(bankDTO);
            }
        }

        public Bank? Get()
        {
            var bank = _repo.Set<Bank>().FirstOrDefault();

            return bank;
        }
    }
}
