using FastBank.Services;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.Repository;
using FastBank.Services.BankService;

namespace FastBank
{
    public static class MenuOptions
    {
        static private User? ActiveUser = null;

        static bool inProgress = true;

        static readonly MenuService _menuService = new MenuService();

        static readonly BankAccountService bankAccountService = new BankAccountService();

        static readonly MessageService _messageService = new MessageService(bankAccountService);

        static public void ShowMainMenu()
        {
            FastBankDbContext db = new FastBankDbContext();
            var repo = new Repository(db);

            while (inProgress)
            {
                Console.Clear();
                _menuService.ShowLogo();

                if (ActiveUser == null)
                {
                    var menuOptions = "Please choose your action: \n 1: For login. 2: For customer registration. 0: for exit";
                    int action = _menuService.CommandRead(3, menuOptions);

                    switch (action)
                    {
                        case 1:
                            {
                                MenuOptions.Login();

                                break;
                            };
                        case 2:
                            {
                                UserRegistration();
                                break;
                            }
                        case 0:
                            {
                                Console.WriteLine("Do you want to exit from the system? Press Y to confirm or any key to cancel...");
                                var confirmKey = Console.ReadKey();
                                if (confirmKey.KeyChar == 'Y')
                                    inProgress = false;
                                break;
                            }
                    }
                }
                else
                {
                    //In depend on customer role we will open different menu with options
                    RenderMenuByRole();
                }
            }

            Console.Clear();
            Console.WriteLine("You successfully exited the system! Pres any key to close programm...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        static public void Login()
        {
            IUserService usersService = new UserService();

            Console.Clear();
            _menuService.ShowLogo();

            Console.WriteLine("\nLogin to FastBank\n");

            var currentEmail = new MenuService().InputEmail();
            if (currentEmail == "quit")
                return;
            Console.WriteLine("Please input password:");

            Console.Write("Password: ");
            var inputPassword = _menuService.PasswordStaredInput();

            var loginUser = usersService.Login(currentEmail, inputPassword);
            if (loginUser != null)
            {
                Console.WriteLine("Authorized");
                if (loginUser.Role == Role.Customer)
                {
                    ActiveUser = new Customer(loginUser);
                }
                else
                {
                    ActiveUser = loginUser;
                }
            }
            else
            {
                ShowMainMenu();
            }
        }

        static public void UserRegistration()
        {
            IUserService userService = new UserService();
            Console.Clear();
            _menuService.ShowLogo();

            var role = Role.Customer;

            Console.WriteLine("\nNew customer registration process is started.\n");

            Console.WriteLine("Please input you name:");
            Console.Write("Name: ");
            var name = Console.ReadLine();

            var email = new MenuService().InputEmail();
            if (email == "quit")
                return;

            Console.WriteLine("Please input you Birthday (format: Year.Month.day):");
            Console.Write("Birthday:");
            string birthdayInput = Console.ReadLine() ?? string.Empty;
            DateTime birthday;
            while (!DateTime.TryParse(birthdayInput, out birthday))
            {
                Console.WriteLine("You inputed wrong Birthday, please use this format: Year.Month.day. Press any key to try again!");
                var keyIsEnter = Console.ReadKey();
                new MenuService().MoveToPreviousLine(keyIsEnter, 2);
                Console.Write("Birthday:");
                birthdayInput = Console.ReadLine() ?? string.Empty;
            }

            Console.WriteLine("Please input you password:");
            Console.Write("Password:");
            var password = _menuService.PasswordStaredInput();

            userService.Add(name, email, birthday, password, role, false);

            MenuOptions.ShowMainMenu();
        }

        public static void RenderMenuByRole()
        {
            Console.Clear();
            _menuService.ShowLogo();

            switch (ActiveUser.Role)
            {
                case Role.Accountant:
                    OpenCustomerMenu();
                    break;
                case Role.Manager:
                    OpenCustomerMenu();
                    break;
                case Role.Customer:
                    OpenCustomerMenu();
                    break;
                case Role.Banker:
                    OpenBankerMenu();
                    break;
                case Role.CustomerService:
                    OpenCustomerServiceMenu();
                    break;
                default:
                    ShowMainMenu();
                    break;
            }
        }

        static public void OpenBankerMenu()
        {
            var bankService = new BankService();

            if (ActiveUser == null || ActiveUser.Role !=  Role.Banker)
            {
                return;
            }

            var menuOptions = $"{{{ActiveUser.Role}}} Welcome {ActiveUser.Name}\n" +
                                 $"\nPlease choose your action: " +
                                  $"\n1: Capital Replenishment. 0: for exit";
            int action = _menuService.CommandRead(2, menuOptions);

            switch (action)
            {
                case 1:
                    {
                        bankService.CapitalReplenishment(ActiveUser);
                        break;
                    }
                case 0:
                    {
                        ActiveUser = null;
                        break;
                    }
                default:
                    break;
            }
            Console.Clear();
        }

        static public void OpenCustomerMenu()
        {
            if (ActiveUser == null || ActiveUser.Role != Role.Customer)
            {
                return;
            }

            var customerBankAccount = bankAccountService.GetBankAccount((Customer)ActiveUser);
            IMessageService MessageService = new MessageService(bankAccountService);

            if (customerBankAccount == null || customerBankAccount.Amount == 0)
            {
                Console.WriteLine($"{{{ActiveUser.Role}}} Welcome {ActiveUser.Name}\n" +
                                  "\nPlease make a deposit at Fast Bank");
                bankAccountService.DepositAmount((Customer)ActiveUser, ref customerBankAccount);
                
                if (customerBankAccount == null || customerBankAccount.Amount == 0)
                {
                    Console.WriteLine("\nYou can't use your account without funds." +
                                      "\nPlease press \"q\" for exit or any key to continue... ");
                    var qkey = Console.ReadKey().KeyChar;
                    if (qkey == 'q')
                        ActiveUser = null;
                }
                Console.Clear();
                _menuService.ShowLogo();
                return;
            }
            else
            {
                var menuOptions = $"{{{ActiveUser.Role}}} Welcome {ActiveUser.Name}\n" + 
                                  $"\nYou bank amount: {customerBankAccount.Amount:0.00} " +
                                  $"\nPlease choose your action: " +
                                  $"\n1: For deposit. 2: For withdraw. 3: Create inquiry. " +
                                    $"4: Check inquiries. 5: Transfer to friend 0: for exit";
                int action = _menuService.CommandRead(6, menuOptions);
                switch (action)
                {
                    case 1:
                        {
                            bankAccountService.DepositAmount((Customer)ActiveUser,ref customerBankAccount);
                            break;
                        }
                    case 2:
                        {
                            bankAccountService.WithdrawAmount(customerBankAccount);
                            break;
                        }
                    case 3:
                        {
                            MessageService.InputMessage(ActiveUser);
                            break;
                        }
                    case 4:
                        {
                            MessageService.ShowMessagesMenu(ActiveUser);
                            break;
                        }
                    case 5:
                        {
                            bankAccountService.TransferMoneyToFriendMenu(customerBankAccount);
                            break;
                        }
                    case 0:
                        {
                            ActiveUser = null;
                            break;  
                        }
                }
            }
            Console.Clear();
        }

        static public void OpenCustomerServiceMenu()
        {
            if (ActiveUser == null || ActiveUser.Role != Role.CustomerService)
            {
                return;
            }

            var messages = _messageService.GetMessages(ActiveUser);

            var messagesCount = messages
                                    .Where(m => m?.MessageStatus == Domain.MessageStatus.Sent 
                                                && m.ReceiverRole == Role.CustomerService)
                                    .Count();

            var menuOptions = $"{{{ActiveUser.Role}}} Welcome {ActiveUser.Name}\n" +
                                $"You have {messagesCount} new message{(messagesCount > 1 ? 's' : string.Empty)}" +
                                $"\nPlease choose your action: " +
                                $"\n1: Manage messages. 0: for exit";
            int action = _menuService.CommandRead(2, menuOptions);

            switch (action)
            {
                case 1:
                    {
                        _messageService.ShowMessagesMenu(ActiveUser, messages);
                        break;
                    }

                case 0:
                    {
                        ActiveUser = null;
                        break;
                    }
            }
            Console.Clear();
        }
    }
}
