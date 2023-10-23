using FastBank.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.Infrastructure.DTOs
{
    [Table("BankAccounts")]
    public class BankAccountDTO
    {
        private BankAccountDTO() { }

        public BankAccountDTO(BankAccount bankAccount)
        {
            BankAccountId = bankAccount.BankAccountId;
            CustomerId = bankAccount.Customer.Id;
            Amount = bankAccount.Amount;
        }

        [Key]
        public Guid BankAccountId { get; private set; }
        
        public Guid CustomerId { get; private set; }

        [ForeignKey(nameof(CustomerId))]
        public CustomerDTO Customer { get; private set; }

        public decimal Amount { get; internal set; }

        public BankAccount ToDomainObj()
        {
            return new BankAccount(BankAccountId, Customer.ToDomainObj(), Amount);
        }
    }

}
