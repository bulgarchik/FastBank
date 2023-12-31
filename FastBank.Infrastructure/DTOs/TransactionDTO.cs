﻿using FastBank.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.Infrastructure.DTOs
{
    [Table("Transactions")]
    public class TransactionDTO
    {
        private TransactionDTO() { }
        public TransactionDTO(Transaction transaction)
        {
            TransactionId = transaction.TransactionId;
            CreatedDate = transaction.CreatedDate;
            CreatedByUserId = transaction.CreatedByUser.Id;
            Amount = transaction.Amount;
            UserNameInitial = transaction.UserNameInitial;
            BankId = transaction.Bank?.Id;
            BankAccountId = transaction.BankAccount?.BankAccountId;
            TransactionType = transaction.TransactionType;
        }
        [Key]
        public Guid TransactionId { get; private set; }
        public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
        public Guid CreatedByUserId { get; private set; }
        
        [ForeignKey(nameof(CreatedByUserId))]
        public UserDTO CreatedByUser { get; private set; } = null!;
        public decimal Amount { get; private set; }
        public string UserNameInitial { get; private set; } = string.Empty;
        public Guid? BankId { get; private set; }
        
        [ForeignKey(nameof(BankId))]
        public BankDTO? Bank { get; private set; }
        public Guid? BankAccountId { get; private set; }

        [ForeignKey(nameof(BankAccountId))]
        public BankAccountDTO? BankAccount { get; private set; }
        public TransactionType TransactionType { get; private set; }

        public Transaction ToDomainObj()
        {
            return new Transaction(
                TransactionId,
                CreatedDate,
                CreatedByUser.ToDomainObj(),
                Amount,
                UserNameInitial,
                Bank?.ToDomainObj(),
                BankAccount?.ToDomainObj(),
                TransactionType);
        }
    }
}
