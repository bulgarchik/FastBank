using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;
using System.Diagnostics;

namespace FastBank.Services
{
    public class TransactionService : ITransactionService
    {
        private const string DATE_TIME_FORMAT = "yyy-MM-dd HH:mm:ss";

        private readonly ITransactionRepository _transactionRepo;
        private readonly IMenuService _menuService;

        public TransactionService()
        {
            _transactionRepo = new TransactionRepository();
            _menuService = new MenuService();
        }

        public List<TransactionsReport> ShowLastTransactionsReports(User user)
        {
            List<TransactionsReport> transactionsReports;

            transactionsReports = _transactionRepo.GetUserLastTransactionsReports(user);

            if (transactionsReports != null && transactionsReports.Count() > 0)
            {
                Console.WriteLine("Transactions reports:\n");

                Console.WriteLine("{0,-4} {1,-40} {2,-30}",
                   "| ID",
                   "| Created by",
                   "| Created on");
                Console.WriteLine(new string('-', 85));

                foreach (var transactionsReport in transactionsReports)
                {
                    Console.WriteLine("{0,-4} {1,-40} {2,-30}",
                        $"| {transactionsReport.Index}",
                        $"| {transactionsReport.CreatedBy.Name}",
                        $"| {TimeZoneInfo.ConvertTimeFromUtc(transactionsReport.CreatedOn, TimeZoneInfo.Local).ToString(DATE_TIME_FORMAT)}");
                }
            }

            return transactionsReports;
        }

        public void ManageTransactionsReport()
        {
            int currentPage = 1;

            List<TransactionsReport> transactionsReports;

            var transactionsReportsCount = _transactionRepo.GetTransactionsReportsCount();

            int totalPages = (int)Math.Ceiling((double)transactionsReportsCount / TransactionRepository.TRANSACTION_PER_PAGE);

            do
            {
                Console.Clear();
                _menuService.ShowLogo();

                transactionsReports = _transactionRepo.GetTransactionsReports(currentPage);

                if (transactionsReports != null && transactionsReports.Count() > 0)
                {
                    Console.WriteLine("Transactions reports:");

                    Console.WriteLine("{0,-4} {1,-40} {2,-30}",
                       "| ID",
                       "| Created by",
                       "| Created on");
                    Console.WriteLine(new string('-', 85));

                    foreach (var transactionsReport in transactionsReports)
                    {
                        Console.WriteLine("{0,-4} {1,-40} {2,-30}",
                            $"| {transactionsReport.Index}",
                            $"| {transactionsReport.CreatedBy.Name}",
                            $"| {TimeZoneInfo.ConvertTimeFromUtc(transactionsReport.CreatedOn, TimeZoneInfo.Local).ToString(DATE_TIME_FORMAT)}");
                    }

                    Console.WriteLine($"\nPage {currentPage}/{totalPages}\n");
                }

                var menuOptions = $"\nPlease choose your action: \n" +
                               $"\n 1: For next page" +
                               $"\n 2: For previous page" +
                               $"\n 3: Open transactions report" +
                               $"\n 4: Show transactions report" +
                               $"\n 0: Exit";
                var commandsCount = 5;
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
                    case 3:
                        {
                            if (transactionsReports != null && transactionsReports.Count > 0)
                            {
                                OpenTransactionsReport(transactionsReports, false);

                            }
                            break;
                        }
                    case 4:
                        {
                            if (transactionsReports != null && transactionsReports.Count > 0)
                            {
                                OpenTransactionsReport(transactionsReports, true);

                            }
                            break;
                        }
                    case 0:
                        {
                            return;
                        }
                }
            }
            while (true);
        }

        public bool OpenTransactionsReport(List<TransactionsReport> transactionsReports, bool showReport = false)
        {
            Console.WriteLine("\nOpening transactions process started...\n");

            var firstIndex = transactionsReports?.First()?.Index;
            var lastIndex = transactionsReports?.Last().Index;
            
            int transactionsReportId;
            do
            {
                Console.WriteLine("Please enter transactions report ID (type 'q' for exit):");
                Console.Write("Transactions report ID: ");
                var inputtransactionsReportId = Console.ReadLine() ?? null;

                if (inputtransactionsReportId == "q")
                    return false;

                if (!int.TryParse(inputtransactionsReportId, out transactionsReportId) || transactionsReportId < firstIndex || transactionsReportId > transactionsReports.Last().Index)
                {
                    Console.WriteLine("Please input correct transactions report ID (press any key to continue...)");
                    var keyIsEnter = Console.ReadKey();
                    new MenuService().MoveToPreviousLine(keyIsEnter, 3);
                }
            } while (transactionsReportId < firstIndex || transactionsReportId > lastIndex);

            var transactionsReport = transactionsReports.Where(x => x.Index == transactionsReportId).FirstOrDefault();
            if (transactionsReport != null && showReport == false)
            {
                Process.Start("notepad.exe", transactionsReport.PathToFile);
            }
            else if (transactionsReport != null && showReport == true)
            {
                try
                {
                    using (var sr = new StreamReader(transactionsReport.PathToFile))
                    {
                        Console.WriteLine(sr.ReadToEnd());
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }

            Console.ReadKey(true);
            return false;
        }

        public void AddTransactionsReport(List<Transaction> transactions, User createdBy)
        {
            var createdOn = DateTime.Now;
            string directoryPath = Directory.GetCurrentDirectory() + "\\TransactionsReports";
            string filePath = Path.Combine(directoryPath, $"TransactionsReport_{createdOn.ToShortDateString()}_{Guid.NewGuid().ToString().Substring(0, 8)}.txt");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine($"\n\t\t\tTransaction Report {createdOn.Date.ToShortDateString()}\n");
                sw.WriteLine("{0,-25} {1,-10} {2,-30} {3, -40}",
                                               "| Date",
                                               "| Amount",
                                               "| Transaction Type",
                                               "| Customer email");
                sw.WriteLine(new string('-', 85));
                foreach (var transaction in transactions)
                {
                    sw.WriteLine("{0,-25} {1,-10} {2,-30} {3, -40}",
                        $"| {TimeZoneInfo.ConvertTimeFromUtc(transaction.CreatedDate, TimeZoneInfo.Local).ToString(DATE_TIME_FORMAT)}",
                        $"| {transaction.Amount}",
                        $"| {transaction.TransactionType.GetDisplayName()}",
                        $"| {transaction.CreatedByUser.Email}");
                }
            }

            _transactionRepo.AddTransactionsReport(
                new TransactionsReport(
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    filePath, createdBy));
        }

        public void CustomerTransactions(User user)
        {
            var transactionsCount = _transactionRepo.GetCustomerTransactionsCount(user);

            int totalPages = (int)Math.Ceiling((double)transactionsCount / TransactionRepository.TRANSACTION_PER_PAGE);

            int currentPage = 1;

            do
            {
                var transactions = _transactionRepo.GetCustomerTransactions(user, currentPage);

                Console.Clear();

                _menuService.ShowLogo();

                ShowTransactionList(transactions);

                Console.WriteLine($"\nPage {currentPage}/{totalPages}\n");

                var menuOptions = $"\nPlease choose your action: \n" +
                               $"\n 1: For next page" +
                               $"\n 2: For previous page" +
                               $"\n 0: Exit";
                var commandsCount = 4;
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

        public void TransactionReport(User user)
        {
            var transactionsCount = _transactionRepo.GetCustomersTransactionsCount();

            int totalPages = (int)Math.Ceiling((double)transactionsCount / TransactionRepository.TRANSACTION_PER_PAGE);

            int currentPage = 1;

            do
            {
                var transactions = _transactionRepo.GetCustomersTransactions(currentPage);

                Console.Clear();

                _menuService.ShowLogo();

                ShowTransactionList(transactions);

                Console.WriteLine($"\nPage {currentPage}/{totalPages}\n");

                var transactionsReports = ShowLastTransactionsReports(user);


                var menuOptions = $"\nPlease choose your action: \n" +
                               $"\n 1: For next page" +
                               $"\n 2: For previous page" +
                               $"\n 3: Save transactions report" +
                               $"\n 4: Open last transactions report" +
                               $"\n 0: Exit";
                var commandsCount = 5;
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
                    case 3:
                        {
                            try
                            {
                                AddTransactionsReport(transactions, user);
                                _menuService.OperationCompleteScreen();
                            }
                            catch (Exception ex)
                            {

                                Console.WriteLine(ex.Message);
                            }

                            break;
                        }
                    case 4:
                        {
                            if (transactionsReports != null && transactionsReports.Count > 0)
                            {
                                OpenTransactionsReport(transactionsReports);

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

        public void ShowTransactionList(List<Transaction> transactions)
        {
            if (!transactions.Any()) return;

            Console.WriteLine("Transactions:\n");

            Console.WriteLine("{0,-25} {1,-10} {2,-30} {3, -40}",
                "| Date",
                "| Amount",
                "| Transaction Type",
                "| Customer email");
            Console.WriteLine(new string('-', 85));

            foreach (var transaction in transactions)
            {
                Console.WriteLine("{0,-25} {1,-10} {2,-30} {3, -40}",
                    $"| {TimeZoneInfo.ConvertTimeFromUtc(transaction.CreatedDate, TimeZoneInfo.Local).ToString(DATE_TIME_FORMAT)}",
                    $"| {transaction.Amount}",
                    $"| {transaction.TransactionType.GetDisplayName()}",
                    $"| {transaction.CreatedByUser.Email}");
            }
        }
    }
}
