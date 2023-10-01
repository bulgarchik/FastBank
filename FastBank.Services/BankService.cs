using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Services
{
    public class BankService : IBankService
    {
        readonly private IRepository _repo;
        public BankService()
        {
            _repo = new FastBank.Repository(new FastBankDbContext());
        }
        
        public void Add(Bank bank)
        {
            if (!_repo.Set<BankDTO>().Any())
            {
                var bankDTO = new BankDTO(10000);
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
