using FastBank.Domain;

namespace FastBank.Services
{
    public interface ITransactionService
    {
        public void TransactionReport(User user);
        public void AddTransactionsReport(List<Transaction> transactions, User createdBy);
        public void ManageTransactionsReport();
        public bool OpenTransactionsReport(List<TransactionsReport> transactionsReports, bool showReport = false);
        public void CustomerTransactions(User user);
        public void ShowTransactionList(List<Transaction> transactions);
        public List<TransactionsReport> ShowLastTransactionsReports(User user);
    }
}
