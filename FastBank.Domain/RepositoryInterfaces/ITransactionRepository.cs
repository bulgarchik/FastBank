namespace FastBank.Domain.RepositoryInterfaces
{
    public interface ITransactionRepository
    {
        public List<Transaction> GetCustomersTransactions(int currentPage=1);

        public int GetCustomerTransactionsCount();

        public void AddTransaction(Transaction transaction);

        public void AddTransactionOrder(TransactionOrder transactionOrder);

        public void AddTransactionsFileReport(TransactionsFileReport transactionsReport);

        public List<TransactionsFileReport> GetTransactionsFileReports(int currentPage = 1);

        public int GetTransactionsFileReportsCount();

        public int GetCustomerTransactionsCount(User user);

        public List<Transaction> GetCustomerTransactions(User user, int currentPage);

        public List<TransactionsFileReport> GetUserLastTransactionsFileReports(User user, int count = 3);
    }
}
