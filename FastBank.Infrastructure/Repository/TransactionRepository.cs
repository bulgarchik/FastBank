using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastBank.Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IRepository _repository;

        public const int TransactionPerPage = 10;

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
                .Skip((currentPage - 1) * TransactionPerPage)
                .Take(TransactionPerPage)
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
    }
}
