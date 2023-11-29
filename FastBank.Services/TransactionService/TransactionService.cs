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

        public List<TransactionsFileReport> ShowLastTransactionsFileReports(User user)
        {
            List<TransactionsFileReport> transactionsFileReports;

            transactionsFileReports = _transactionRepo.GetUserLastTransactionsFileReports(user);

            if (transactionsFileReports != null && transactionsFileReports.Count() > 0)
            {
                Console.WriteLine("Transactions file reports:\n");

                Console.WriteLine("{0,-4} {1,-40} {2,-30}",
                   "| ID",
                   "| Created by",
                   "| Created on");
                Console.WriteLine(new string('-', 85));

                foreach (var transactionsFileReport in transactionsFileReports)
                {
                    Console.WriteLine("{0,-4} {1,-40} {2,-30}",
                        $"| {transactionsFileReport.Index}",
                        $"| {transactionsFileReport.CreatedBy.Name}",
                        $"| {TimeZoneInfo.ConvertTimeFromUtc(transactionsFileReport.CreatedOn, TimeZoneInfo.Local).ToString(DATE_TIME_FORMAT)}");
                }
            }

            return transactionsFileReports;
        }

        public void ManageTransactionsFileReport()
        {
            int currentPage = 1;

            List<TransactionsFileReport> transactionFileReports;

            var transactionsFileReportsCount = _transactionRepo.GetTransactionsFileReportsCount();

            int totalPages = (int)Math.Ceiling((double)transactionsFileReportsCount / TransactionRepository.TRANSACTIONS_PER_PAGE);

            do
            {
                Console.Clear();
                _menuService.ShowLogo();

                transactionFileReports = _transactionRepo.GetTransactionsFileReports(currentPage);

                if (transactionFileReports != null && transactionFileReports.Count() > 0)
                {
                    Console.WriteLine("Transactions file reports:");

                    Console.WriteLine("{0,-4} {1,-40} {2,-30}",
                       "| ID",
                       "| Created by",
                       "| Created on");
                    Console.WriteLine(new string('-', 85));

                    foreach (var transactionsFileReport in transactionFileReports)
                    {
                        Console.WriteLine("{0,-4} {1,-40} {2,-30}",
                            $"| {transactionsFileReport.Index}",
                            $"| {transactionsFileReport.CreatedBy.Name}",
                            $"| {TimeZoneInfo.ConvertTimeFromUtc(transactionsFileReport.CreatedOn, TimeZoneInfo.Local).ToString(DATE_TIME_FORMAT)}");
                    }

                    Console.WriteLine($"\nPage {currentPage}/{totalPages}\n");
                }

                var menuOptions = $"\nPlease choose your action: \n" +
                               $"\n 1: Next page" +
                               $"\n 2: Previous page" +
                               $"\n 3: Open transactions file report" +
                               $"\n 4: Show transactions file report" +
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
                            if (transactionFileReports != null && transactionFileReports.Count > 0)
                            {
                                OpenTransactionsFileReport(transactionFileReports, false);

                            }
                            break;
                        }
                    case 4:
                        {
                            if (transactionFileReports != null && transactionFileReports.Count > 0)
                            {
                                OpenTransactionsFileReport(transactionFileReports, true);

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

        public bool OpenTransactionsFileReport(List<TransactionsFileReport> transactionsFileReports, bool showReport = false)
        {
            Console.WriteLine("\nOpening transactions file report process started...\n");

            var firstIndex = transactionsFileReports?.First()?.Index;
            var lastIndex = transactionsFileReports?.Last().Index;
            
            int transactionsFileReportId;
            do
            {
                Console.WriteLine("Please enter transactions file report ID (type 'q' for exit):");
                Console.Write("Transactions file report ID: ");
                var inputTransactionsFileReportId = Console.ReadLine() ?? null;

                if (inputTransactionsFileReportId == "q")
                    return false;

                if (!int.TryParse(inputTransactionsFileReportId, out transactionsFileReportId) || transactionsFileReportId < firstIndex || transactionsFileReportId > transactionsFileReports.Last().Index)
                {
                    Console.WriteLine("Please input correct transactions file report ID (press any key to continue...)");
                    var keyIsEnter = Console.ReadKey();
                    new MenuService().MoveToPreviousLine(keyIsEnter, 3);
                }
            } while (transactionsFileReportId < firstIndex || transactionsFileReportId > lastIndex);

            var transactionsFileReport = transactionsFileReports?.Where(x => x.Index == transactionsFileReportId).FirstOrDefault();
            if (transactionsFileReport != null && showReport == false)
            {
                Process.Start("notepad.exe", transactionsFileReport.PathToFile);
            }
            else if (transactionsFileReport != null && showReport == true)
            {
                try
                {
                    using (var sr = new StreamReader(transactionsFileReport.PathToFile))
                    {
                        Console.WriteLine(sr.ReadToEnd());
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("The transactions file report could not be read:");
                    Console.WriteLine(e.Message);
                }
            }

            Console.ReadKey(true);
            return false;
        }

        public void AddTransactionsFileReport(List<Transaction> transactions, User createdBy)
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

            _transactionRepo.AddTransactionsFileReport(
                new TransactionsFileReport(
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    filePath, createdBy));
        }

        public void CustomerTransactions(User user)
        {
            var transactionsCount = _transactionRepo.GetCustomerTransactionsCount(user);

            int totalPages = (int)Math.Ceiling((double)transactionsCount / TransactionRepository.TRANSACTIONS_PER_PAGE);

            int currentPage = 1;

            do
            {
                var transactions = _transactionRepo.GetCustomerTransactions(user, currentPage);

                Console.Clear();

                _menuService.ShowLogo();

                ShowTransactionList(transactions);

                Console.WriteLine($"\nPage {currentPage}/{totalPages}\n");

                var menuOptions = $"\nPlease choose your action: \n" +
                               $"\n 1: Next page" +
                               $"\n 2: Previous page" +
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
            var transactionsCount = _transactionRepo.GetCustomerTransactionsCount();

            int totalPages = (int)Math.Ceiling((double)transactionsCount / TransactionRepository.TRANSACTIONS_PER_PAGE);

            int currentPage = 1;

            do
            {
                var transactions = _transactionRepo.GetCustomersTransactions(currentPage);

                Console.Clear();

                _menuService.ShowLogo();

                ShowTransactionList(transactions);

                Console.WriteLine($"\nPage {currentPage}/{totalPages}\n");

                var transactionsFileReports = ShowLastTransactionsFileReports(user);


                var menuOptions = $"\nPlease choose your action: \n" +
                               $"\n 1: Next page" +
                               $"\n 2: Previous page" +
                               $"\n 3: Save transactions file report" +
                               $"\n 4: Open last transactions file report" +
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
                                AddTransactionsFileReport(transactions, user);
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
                            if (transactionsFileReports != null && transactionsFileReports.Count > 0)
                            {
                                OpenTransactionsFileReport(transactionsFileReports);

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
