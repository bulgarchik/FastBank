using FastBank.Domain;

namespace FastBank.Services
{
    public interface ITransactionService
    {
        public void TransactionReport(User user);
        public void AddTransactionsFileReport(List<Transaction> transactions, User createdBy);
        public void ManageTransactionsFileReport();
        public bool OpenTransactionsFileReport(List<TransactionsFileReport> transactionsReports, bool showReport = false);
        public void CustomerTransactions(User user);
        public void ShowTransactionList(List<Transaction> transactions);
        public List<TransactionsFileReport> ShowLastTransactionsFileReports(User user);
    }
}
