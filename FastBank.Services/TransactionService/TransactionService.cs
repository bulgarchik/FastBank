using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;

namespace FastBank.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepo;

        public TransactionService()
        {
            _transactionRepo = new TransactionRepository();
        }
        public void AddTranscation(
            User createdByUser,
            decimal amount,
            Bank? bank,
            BankAccount? bankAccount,
            TransactionType transactionType)
        {
            var transaction = new Transaction(
                                    new Guid(),
                                    DateTime.UtcNow,
                                    createdByUser,
                                    amount,
                                    createdByUser.Name.Trim()[..2],
                                    bank,
                                    bankAccount,
                                    transactionType);
            _transactionRepo.AddTransaction(transaction);
        }
    }
}
