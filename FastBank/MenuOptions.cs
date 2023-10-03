using FastBank.Services;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.Repository;
using System.Text.RegularExpressions;

namespace FastBank
{
    public static class MenuOptions
    {
        static public Customer? ActiveCustomer = null;

        static bool inProgress = true;

        public static int CommandRead(Regex regPattern, string menuOptions)
        {
            Console.WriteLine(menuOptions);
            string? inputCommand = Console.ReadLine();
            while (!regPattern.IsMatch(inputCommand ?? ""))
            {
                Console.WriteLine("\nERROR: Please input correct command from menu. (press any key to continue..)");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine(menuOptions);
                inputCommand = Console.ReadLine();

            }
            return Convert.ToInt32(inputCommand);
        }

        static public void ShowMainMenu()
        {
            FastBankDbContext db = new FastBankDbContext();
            var repo = new Repository(db);

            while (inProgress)
            {
                Console.Clear();
                if (ActiveCustomer == null)
                {
                    var menuOptions = "Please choose your action: \n 1: For login. 2: For registration. 0: for exit";
                    int action = CommandRead(new Regex("^[012]{1}$"), menuOptions);

                    switch (action)
                    {
                        case 1:
                            {
                                MenuOptions.Login();

                                break;
                            };
                        case 2:
                            {
                                CustomerRegistration();
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
            ICustomerService customerService = new CustomerService();

            Console.Clear();

            Console.WriteLine("Please input login(email):");
            var currentEmail = Console.ReadLine() ?? "";
            Console.WriteLine("Please input password:");
            var inputPassword = Console.ReadLine() ?? "";
            var loginCustomer = customerService.Login(currentEmail, inputPassword);
            if (loginCustomer != null)
            {
                Console.WriteLine("Authorized");
                ActiveCustomer = loginCustomer;
            }
            else
            {
                ShowMainMenu();
            }
        }
        static public void CustomerRegistration()
        {
            ICustomerService customerService = new CustomerService();
            Console.Clear();
            var role = Roles.Customer;

            Console.WriteLine("Please input registration data about you:");

            Console.WriteLine("Please input you name:");
            var name = Console.ReadLine();

            Console.WriteLine("Please input you email:");
            var email = Console.ReadLine();

            Console.WriteLine("Please input you Birthday:");
            var birthday = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Please input you password:");
            var password = Console.ReadLine();

            customerService.Add(name, email, birthday, password, role, false);

            MenuOptions.ShowMainMenu();
        }

        public static void RenderMenuByRole()
        {
            Console.Clear();
            Console.WriteLine($"Welcome to FastBank as {ActiveCustomer.Role}");
            switch (ActiveCustomer.Role)
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
            if (ActiveCustomer == null)
            {
                return;
            }
            Console.WriteLine("Please choose your action:");
            Console.WriteLine(" 0: for exit");
            int action = Convert.ToInt32(Console.ReadLine());
            bool activeScreen = true;
            while (activeScreen)
            {
                switch (action)
                {
                    case 0:
                        {
                            activeScreen = false;
                            ActiveCustomer = null;
                            break;
                        }
                }
            }
        }
    }
}
