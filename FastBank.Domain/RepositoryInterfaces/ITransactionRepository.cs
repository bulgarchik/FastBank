namespace FastBank.Domain.RepositoryInterfaces
{
    public interface ITransactionRepository
    {
        public List<Transaction> GetCustomersTransactions();

        public void AddTransaction(Transaction transaction);

        public void AddTransactionOrder(TransactionOrder transactionOrder);
    }
}
