using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Domain
{
    public class BankAccount
    {
       public BankAccount(Guid bankAccountId, Customer customer) 
        {
            BankAccountId = bankAccountId;
            Customer = customer;
        }

        public Guid BankAccountId { get; private set; }

        public Customer Customer { get; private set; }

        public decimal Amount { get; private set; }
    }
}
