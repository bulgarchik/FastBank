using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.Repository;

namespace FastBank.Services.BankService
{
    public class BankService : IBankService
    {
        private readonly IRepository _repo;
        public BankService()
        {
            _repo = new Repository(new FastBankDbContext());
        }

        public Bank? Get()
        {
            var bank = _repo.Set<BankDTO>().Select(a => a.ToDomainObj()).FirstOrDefault();

            return bank;
        }
    }
}
