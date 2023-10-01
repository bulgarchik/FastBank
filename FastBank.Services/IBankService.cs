using FastBank.Domain;
using FastBank.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Services
{
    public interface IBankService
    {
        public void Add(Bank bank);

        public Bank? Get();
    }
}
