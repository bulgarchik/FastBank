namespace FastBank.Domain.RepositoryInterfaces
{
    public interface ITransactionRepository
    {
        public List<Transaction> GetCustomersTransactions(int currentPage=1);

        public int GetCustomersTransactionsCount();

        public void AddTransaction(Transaction transaction);

        public void AddTransactionOrder(TransactionOrder transactionOrder);

        public void AddTransactionsReport(TransactionsReport transactionsReport);

        public List<TransactionsReport> GetTransactionsReports(int currentPage = 1);

        public int GetTransactionsReportsCount();

        public int GetCustomerTransactionsCount(User user);
        public List<Transaction> GetCustomerTransactions(User user, int currentPage = 1);

    }
}
