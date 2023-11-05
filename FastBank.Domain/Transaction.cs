namespace FastBank.Domain
{
    public class Transaction
    {
        public Transaction(
            Guid transactionId,
            DateTime createdDate,
            User createdByUser,
            decimal amount,
            string userNameInitial,
            Bank? bank,
            BankAccount? bankAccount,
            TransactionType transactionType)
        {
            TransactionId = transactionId;
            CreatedDate = createdDate;
            CreatedByUser = createdByUser;
            Amount = amount;
            UserNameInitial = userNameInitial;
            Bank = bank;
            BankAccount = bankAccount;
            TransactionType = transactionType;
        }

        public Guid TransactionId { get; private set; }
        public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
        public User CreatedByUser { get; private set; }
        public decimal Amount { get; private set; }
        public string UserNameInitial { get; private set; }
        public Bank? Bank { get; private set; }
        public BankAccount? BankAccount { get; private set; }
        public TransactionType TransactionType { get; private set; }
    }
}

public enum TransactionType
{
    BankTransaction,
    BankAccountTransaction,
}
