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

        public void CapitalReplenishment(User user)
        {
            //Here we should
            //1.Validate user rights
            //2.Show current bank capital amount
            //3.Ask about amount to be replenished
            //4 Confirm the operation
            //5.Create transaction about action
            

            throw new NotImplementedException();
        }

        public Bank? Get()
        {
            var bank = _repo.Set<BankDTO>().Select(a => a.ToDomainObj()).FirstOrDefault();

            return bank;
        }
    }
}
