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
            UserId = bankAccount.Customer.Id;
            Amount = bankAccount.Amount;
        }

        [Key]
        public Guid BankAccountId { get; private set; }
        
        public Guid UserId { get; private set; }

        [ForeignKey(nameof(UserId))]
        public UserDTO User { get; private set; }

        public decimal Amount { get; internal set; }

        public BankAccount ToDomainObj()
        {
            return new BankAccount(BankAccountId, User.ToDomainObj(), Amount);
        }
    }

}
