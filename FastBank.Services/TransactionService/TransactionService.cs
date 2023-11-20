using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;

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
            var transactionsCount = _transactionRepo.GetCustomersTransactionsCount();

            int totalPages = (int)Math.Ceiling((double)transactionsCount / TransactionRepository.TransactionPerPage);

            int currentPage = 1;

            do
            {
                var transactions = _transactionRepo.GetCustomersTransactions(currentPage);

                Console.Clear();

                _menuService.ShowLogo();

                Console.WriteLine("{0,-25} {1,-10} {2,-30} {3, -40}",
                "| Date",
                "| Amount",
                "| Transaction Type",
                "| Customer email");
                Console.WriteLine(new string('-', 85));

                foreach (var transaction in transactions)
                {
                    Console.WriteLine("{0,-25} {1,-10} {2,-30} {3, -40}",
                        $"| {transaction.CreatedDate}",
                        $"| {transaction.Amount}",
                        $"| {transaction.TransactionType.GetDisplayName()}",
                        $"| {transaction.CreatedByUser.Email}");
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
