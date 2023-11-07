namespace FastBank.Domain.RepositoryInterfaces
{
    public interface ITransactionRepository
    {
        public void AddTransaction(Transaction transaction);

        public void AddTransactionOrder(TransactionOrder transactionOrder);
    }
}
