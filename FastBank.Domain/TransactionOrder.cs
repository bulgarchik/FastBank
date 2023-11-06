using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Domain
{
    public class TransactionOrder
    {
        public TransactionOrder(
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

        public Guid TransactionOrderId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public BankAccount? FromBankAccount { get; private set; }
        public BankAccount? ToBankAccount { get; private set; }
        public Bank? Bank { get; private set; }
        public User OrderedByUser { get; private set; } = null!;
        public decimal Amount { get; private set; }
    }

    public enum TransactionOrderStatus
    {
        Opened,
        Completed,
        Canceled
    }
}
