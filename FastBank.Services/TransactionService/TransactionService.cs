using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;

namespace FastBank.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepo;
        
        public TransactionService()
        {
            _transactionRepo = new TransactionRepository();
        }

        public void TransactionReport(User user)
        {
            if (user == null || user.Role != Role.Manager)
            {
                return;
            }

            var transactions = _transactionRepo.GetCustomersTransactions();

            Console.WriteLine("{0,-25} {1,-10} {2,-30} {3, -40}",
                "| Date",
                "| Amount",
                "| Transaction Type",
                "| Customer email");
            Console.WriteLine(new string('-', 85));

            foreach ( var transaction in transactions )
            {
                Console.WriteLine("{0,-25} {1,-10} {2,-30} {3, -40}",
                    $"| {transaction.CreatedDate}",
                    $"| {transaction.Amount}",
                    $"| {transaction.TransactionType.GetDisplayName()}",
                    $"| {transaction.CreatedByUser.Email}");
            }
            Console.ReadKey();
        }
    }
}
