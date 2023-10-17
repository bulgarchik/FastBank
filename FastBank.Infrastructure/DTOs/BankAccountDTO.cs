using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Infrastructure.DTOs
{
    public class BankAccountDTO
    {
        private BankAccountDTO() { }

        public BankAccountDTO(Guid bankAccountId, Customer customer, decimal amount)
        {
            BankAccountId = bankAccountId;
            CustomerDTO = new CustomerDTO(customer);
            Amount = amount;
        }

        public Guid BankAccountId { get; private set; }

        public CustomerDTO CustomerDTO { get; private set; }

        public decimal Amount { get; private set; }

        public BankAccount ToDomainObj()
        {
            return new BankAccount(BankAccountId, CustomerDTO.ToDomainObj());
        }
    }

}
