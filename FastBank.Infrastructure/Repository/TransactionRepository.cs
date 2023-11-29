using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastBank.Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IRepository _repository;

        public const int TRANSACTIONS_PER_PAGE = 3;

        public TransactionRepository()
        {
            _repository = new Repository(new Context.FastBankDbContext());
        }

        public List<Transaction> GetCustomersTransactions(int currentPage = 1)
        {
            var transactions = _repository.Set<TransactionDTO>()
                .Include(t => t.CreatedByUser)
                .Where(t => t.CreatedByUser.Role == Role.Customer.ToString())
                .OrderBy(t => t.CreatedDate)
                .Skip((currentPage - 1) * TRANSACTIONS_PER_PAGE)
                .Take(TRANSACTIONS_PER_PAGE)
                .Select(t => t.ToDomainObj())
                .ToList();

            return transactions;
        }

        public int GetCustomerTransactionsCount()
        {
            var transactionsCount = _repository.Set<TransactionDTO>()
                .Include(t => t.CreatedByUser)
                .Where(t => t.CreatedByUser.Role == Role.Customer.ToString())
                .Count();

            return transactionsCount;
        }

        public int GetCustomerTransactionsCount(User user)
        {
            var transactionsCount = _repository.Set<TransactionDTO>()
                .Include(t => t.CreatedByUser)
                .Where(t => t.CreatedByUser.Role == Role.Customer.ToString() && t.CreatedByUser.UserId == user.Id)
                .Count();

            return transactionsCount;
        }

        public List<Transaction> GetCustomerTransactions(User user, int currentPage = 1)
        {
            var transactions = _repository.Set<TransactionDTO>()
                .Include(t => t.CreatedByUser)
                .Where(t => t.CreatedByUser.Role == Role.Customer.ToString() && t.CreatedByUser.UserId == user.Id)
                .OrderBy(t => t.CreatedDate)
                .Skip((currentPage - 1) * TRANSACTIONS_PER_PAGE)
                .Take(TRANSACTIONS_PER_PAGE)
                .Select(t => t.ToDomainObj())
                .ToList();

            return transactions;
        }

        public void AddTransaction(Transaction transaction)
        {
            _repository.Add<TransactionDTO>(new TransactionDTO(transaction));
        }

        public void AddTransactionOrder(TransactionOrder transactionOrder)
        {
            _repository.Add<TransactionOrderDTO>(new TransactionOrderDTO(transactionOrder));
        }
    
        public void AddTransactionsFileReport(TransactionsFileReport transactionsFileReport)
        {
            _repository.Add<TransactionsFileReportDTO>(new TransactionsFileReportDTO(transactionsFileReport));
        }

        public List<TransactionsFileReport> GetTransactionsFileReports(int currentPage = 1)
        {
            var currentPageTransactionsFileReportsIndex = (currentPage - 1) * TRANSACTIONS_PER_PAGE + 1;

            var transactionsReports =  _repository.Set<TransactionsFileReportDTO>()
                        .Include(tr => tr.CreatedBy)
                        .OrderBy(tr => tr.CreatedOn)
                        .Skip((currentPage - 1) * TRANSACTIONS_PER_PAGE)
                        .Take(TRANSACTIONS_PER_PAGE)
                        .Select(tr => tr.ToDomainObj())
                        .ToList();

            foreach (var item in transactionsReports)
            {
                item.SetIndex(currentPageTransactionsFileReportsIndex++);
            }

            return transactionsReports;
        }

        public List<TransactionsFileReport> GetUserLastTransactionsFileReports(User user, int count = 3)
        {
            var currentPageTransactionsFileReportsIndex = 1;

            var transactionsFileReports = _repository.Set<TransactionsFileReportDTO>()
                        .Include(tr => tr.CreatedBy)
                        .OrderByDescending(tr => tr.CreatedOn)
                        .Take(count)
                        .Select(tr => tr.ToDomainObj())
                        .ToList();

            foreach (var item in transactionsFileReports)
            {
                item.SetIndex(currentPageTransactionsFileReportsIndex++);
            }

            return transactionsFileReports;
        }

        public int GetTransactionsFileReportsCount()
        {
            return _repository.Set<TransactionsFileReportDTO>()
                        .Include(tr => tr.CreatedBy)
                        .Count();
        }
    }
}
