using FastBank.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Infrastructure.DTOs
{
    [Table("TransactionOrders")]
    public class TransactionOrderDto
    {
        public TransactionOrderDto(
            Guid transactionOrderId,
            DateTime createdDate,
            TransactionType transactionType,
            BankAccount? fromBankAccount,
            BankAccount? toBankAccount,
            Bank? bank,
            User orderByUser,
            decimal amount)
        {
            TransactionOrderId = transactionOrderId;
            CreatedDate = createdDate;
            TransactionType = transactionType;
            FromBankAccount = fromBankAccount;
            ToBankAccount = toBankAccount;
            Bank = bank;
            OrderedByUser = orderByUser;
            Amount = amount;
        }
        [Key]
        public Guid TransactionOrderId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public Guid? FromBankAccountId { get; private set; }
        [ForeignKey(nameof(FromBankAccountId))]
        public BankAccount? FromBankAccount { get; private set; }
        public Guid? ToBankAccountId {  get; private set; }
        [ForeignKey(nameof(ToBankAccountId))]
        public BankAccount? ToBankAccount { get; private set; }
        public Guid BankId { get; private set; }
        [ForeignKey(nameof(BankId))]
        public Bank? Bank { get; private set; }
        public Guid? OrderedByUserId { get; private set; }
        [ForeignKey(nameof(OrderedByUserId))]
        public User OrderedByUser { get; private set; } = null!;
        public decimal Amount { get; private set; }

        public TransactionOrder ToDomeinObj()
        {
            return new TransactionOrder(
                TransactionOrderId,
                CreatedDate,
                TransactionType,
                FromBankAccount,
                ToBankAccount,
                Bank,
                OrderedByUser,
                Amount);
        }
    }
}
