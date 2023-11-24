using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastBank.Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IRepository _repository;

        public const int TRANSACTION_PER_PAGE = 3;

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
                .Skip((currentPage - 1) * TRANSACTION_PER_PAGE)
                .Take(TRANSACTION_PER_PAGE)
                .Select(t => t.ToDomainObj())
                .ToList();

            return transactions;
        }

        public int GetCustomersTransactionsCount()
        {
            var transactionsCount = _repository.Set<TransactionDTO>()
                .Include(t => t.CreatedByUser)
                .Where(t => t.CreatedByUser.Role == Role.Customer.ToString())
                .OrderBy(t => t.CreatedDate)
                .Select(t => t.ToDomainObj())
                .Count();

            return transactionsCount;
        }

        public int GetCustomerTransactionsCount(User user)
        {
            var transactionsCount = _repository.Set<TransactionDTO>()
                .Include(t => t.CreatedByUser)
                .Where(t => t.CreatedByUser.Role == Role.Customer.ToString() && t.CreatedByUser.UserId == user.Id)
                .OrderBy(t => t.CreatedDate)
                .Select(t => t.ToDomainObj())
                .Count();

            return transactionsCount;
        }

        public List<Transaction> GetCustomerTransactions(User user, int currentPage = 1)
        {
            var transactions = _repository.Set<TransactionDTO>()
                .Include(t => t.CreatedByUser)
                .Where(t => t.CreatedByUser.Role == Role.Customer.ToString() && t.CreatedByUser.UserId == user.Id)
                .OrderBy(t => t.CreatedDate)
                .Skip((currentPage - 1) * TRANSACTION_PER_PAGE)
                .Take(TRANSACTION_PER_PAGE)
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
    
        public void AddTransactionsReport(TransactionsReport transactionsReport)
        {
            _repository.Add<TransactionsReportDTO>(new TransactionsReportDTO(transactionsReport));
        }

        public List<TransactionsReport> GetTransactionsReports(int currentPage = 1)
        {
            var currentPageTransactionsReportsIndex = (currentPage - 1) * TRANSACTION_PER_PAGE + 1;

            var transactionsReports =  _repository.Set<TransactionsReportDTO>()
                        .Include(tr => tr.CreatedBy)
                        .OrderBy(tr => tr.CreatedOn)
                        .Skip((currentPage - 1) * TRANSACTION_PER_PAGE)
                        .Take(TRANSACTION_PER_PAGE)
                        .Select(tr => tr.ToDomainObj())
                        .ToList();

            foreach (var item in transactionsReports)
            {
                item.Index = currentPageTransactionsReportsIndex++;
            }

            return transactionsReports;
        }

        public int GetTransactionsReportsCount()
        {
            return _repository.Set<TransactionsReportDTO>()
                        .Include(tr => tr.CreatedBy)
                        .Count();
        }
    }
}
