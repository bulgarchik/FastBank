using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastBank.Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IRepository _repository;

        public TransactionRepository()
        {
            _repository = new Repository(new Context.FastBankDbContext());
        }

        public List<Transaction> GetCustomersTransactions()
        {
            var Transactions = _repository.Set<TransactionDTO>()
                .Include(t => t.CreatedByUser)
                .Where(t => t.CreatedByUser.Role == Role.Customer.ToString())
                .OrderBy(t => t.CreatedDate)
                .Select(t => t.ToDomainObj())
                .ToList();
                       
            return Transactions;
        }

        public void AddTransaction(Transaction transaction)
        {
            _repository.Add<TransactionDTO>(new TransactionDTO(transaction));
        }

        public void AddTransactionOrder(TransactionOrder transactionOrder)
        {
            _repository.Add<TransactionOrderDto>(new TransactionOrderDto(transactionOrder));
        }
    }
}
