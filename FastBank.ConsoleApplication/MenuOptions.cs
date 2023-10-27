using FastBank.Services;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.Repository;
using System.Text.RegularExpressions;
using FastBank.Services.BankAccountService;
using FastBank.Services.MessageService;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FastBank
{
    public static class MenuOptions
    {
        static private User? ActiveUser = null;

        static bool inProgress = true;

        static readonly MenuService _menuService = new MenuService();

        static public void ShowMainMenu()
        {
            FastBankDbContext db = new FastBankDbContext();
            var repo = new Repository(db);

            while (inProgress)
            {
                Console.Clear();
                if (ActiveUser == null)
                {
                    var menuOptions = "Please choose your action: \n 1: For login. 2: For customer registration. 0: for exit";
                    int action = _menuService.CommandRead(new Regex("^[012]{1}$"), menuOptions);

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
        }

        static public void Login()
        {
            IUserService usersService = new UserService();

            Console.Clear();

            Console.WriteLine("\nLogin to FastBank\n");

            Console.WriteLine("Please input login(email):");
            var currentEmail = Console.ReadLine() ?? "";
            Console.WriteLine("Please input password:");

            var inputPassword = _menuService.PasswordStaredInput();

            var loginUser = usersService.Login(currentEmail, inputPassword);
            if (loginUser != null)
            {
                Console.WriteLine("Authorized");
                if (loginUser.Role == Roles.Customer)
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

            var role = Roles.Customer;

            Console.WriteLine("\nNew customer registration process is started.\n");

            Console.WriteLine("Please input registration data about customer:");

            Console.WriteLine("Please input you name:");
            var name = Console.ReadLine();

            Console.WriteLine("Please input you email:");
            var email = Console.ReadLine();

            Console.WriteLine("Please input you Birthday (format: Year.Month.day):");
            string birthdayInput = Console.ReadLine() ?? "";
            DateTime birthday;
            while (!DateTime.TryParse(birthdayInput, out birthday))
            {
                Console.WriteLine("You inputed wrong Birthday, please use this format: Year.Month.day. Press any key to try again!");
                var keyIsEnter = Console.ReadKey();
                new MenuService().MoveToPreviousLine(keyIsEnter, 2);
                birthdayInput = Console.ReadLine() ?? "";
            }

            Console.WriteLine("Please input you password:");
            var password = _menuService.PasswordStaredInput();

            userService.Add(name, email, birthday, password, role, false);

            MenuOptions.ShowMainMenu();
        }

        public static void RenderMenuByRole()
        {
            Console.Clear();
            switch (ActiveUser.Role)
            {
                case Roles.Accountant:
                    OpenCustomerMenu();
                    break;
                case Roles.Manager:
                    OpenCustomerMenu();
                    break;
                case Roles.Customer:
                    OpenCustomerMenu();
                    break;
                case Roles.Banker:
                    OpenCustomerMenu();
                    break;
                case Roles.CustomerService:
                    OpenCustomerMenu();
                    break;
                default:
                    ShowMainMenu();
                    break;
            }
        }

        static public void OpenCustomerMenu()
        {
            if (ActiveUser == null || ActiveUser is not Customer)
            {
                return;
            }

            var bankAccountService = new BankAccountService();
            var customerBankAccount = bankAccountService.GetBankAccount((Customer)ActiveUser);
            IMessageService MessageService = new MessageService();

            if (customerBankAccount == null || customerBankAccount.Amount == 0)
            {
                Console.WriteLine($"Welcome {ActiveUser.Name} as {ActiveUser.Role} of FastBank" +
                                  "\nPlease make a deposit at Fast Bank");
                bankAccountService.DepositAmount((Customer)ActiveUser, customerBankAccount);
                Console.Clear();
                return;
            }
            else
            {
                var menuOptions = $"Welcome {ActiveUser.Name} as {ActiveUser.Role} of FastBank" +
                                  $"\nYou bank amount: {customerBankAccount.Amount:0.00} " +
                                  $"\nPlease choose your action: " +
                                  $"\n1: For deposit. 2: For withdraw. 3: For inquiry. 4. Check inquiries  0: for exit";
                int action = _menuService.CommandRead(new Regex("^[01234]{1}$"), menuOptions);
                switch (action)
                {
                    case 1:
                        {
                            bankAccountService.DepositAmount((Customer)ActiveUser, customerBankAccount);
                            break;
                        }
                    case 2:
                        {
                            bankAccountService.WithdrawAmount((Customer)ActiveUser, customerBankAccount);
                            break;
                        }
                    case 3:
                        {
                            MessageService.InputMessage(ActiveUser);
                            break;
                        }
                    case 4:
                        {
                            MessageService.GetMessages(ActiveUser);
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
    }
}
