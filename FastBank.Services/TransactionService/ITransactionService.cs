using FastBank.Domain;

namespace FastBank.Services
{
    public interface ITransactionService
    {
        public Transaction AddTranscation(
            User createdByUser,
            decimal amount,
            Bank? bank,
            BankAccount? bankAccount,
            TransactionType transactionType);

        public void ConfirmTransactionOrder(TransactionOrder transactionOrder);

        public void AddTransactionOrder(TransactionOrder transactionOrder);
    }
}
