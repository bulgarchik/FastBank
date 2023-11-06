using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;

namespace FastBank.Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IRepository _repository;

        public TransactionRepository()
        {
            _repository = new Repository(new Context.FastBankDbContext());
        }

        public void AddTransaction(Transaction transaction)
        {
            _repository.Add<TransactionDTO>(new TransactionDTO(transaction));
        }
    }
}
