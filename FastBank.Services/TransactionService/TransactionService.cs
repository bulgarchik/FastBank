using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FastBank.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepo;
        private readonly IMenuService _menuService;

        public TransactionService()
        {
            _transactionRepo = new TransactionRepository();
            _menuService = new MenuService();
        }

        public void TransactionReport(User user)
        {
            if (user == null || user.Role != Role.Manager)
            {
                return;
            }

            var transactions = _transactionRepo.GetCustomersTransactions();

            int transactionsPerPage = 3;
            int currentPage = 1;
            int totalPages = (int)Math.Ceiling((double)transactions.Count / transactionsPerPage);

            do
            {
                Console.Clear();

                _menuService.ShowLogo();

                int startIndex = (currentPage - 1) * transactionsPerPage;
                int endIndex = Math.Min(startIndex + transactionsPerPage - 1, transactions.Count - 1);

                Console.WriteLine("{0,-25} {1,-10} {2,-30} {3, -40}",
                "| Date",
                "| Amount",
                "| Transaction Type",
                "| Customer email");
                Console.WriteLine(new string('-', 85));

                for (int i = startIndex; i <= endIndex; i++)
                {
                    Console.WriteLine("{0,-25} {1,-10} {2,-30} {3, -40}",
                        $"| {transactions[i].CreatedDate}",
                        $"| {transactions[i].Amount}",
                        $"| {transactions[i].TransactionType.GetDisplayName()}",
                        $"| {transactions[i].CreatedByUser.Email}");
                }

                Console.WriteLine($"\nPage {currentPage}/{totalPages}\n");

                var menuOptions = $"\nPlease choose your action: \n" +
                               $"\n 1: For next page" +
                               $"\n 2: For previous page" +
                               $"\n 0: Exit";
                var commandsCount = 3;
                int action = _menuService.CommandRead(commandsCount, menuOptions);

                switch (action)
                {
                    case 1:
                        {
                            if (currentPage < totalPages)
                            {
                                currentPage++;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (currentPage > 1)
                            {
                                currentPage--;
                            }
                            break;
                        }
                    case 0:
                        {
                            return;
                        }
                }

            } while (true);
        }
    }
}
