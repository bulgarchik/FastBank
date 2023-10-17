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
            CustomerDTO = new CustomerDTO(bankAccount.Customer);
            Amount = bankAccount.Amount;
        }

        [Key]
        public Guid BankAccountId { get; private set; }

        public CustomerDTO CustomerDTO { get; private set; }

        public decimal Amount { get; private set; }

        public BankAccount ToDomainObj()
        {
            return new BankAccount(BankAccountId, CustomerDTO.ToDomainObj(), Amount);
        }
    }

}
