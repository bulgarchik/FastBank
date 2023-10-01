using FastBank.Infrastructure;
using FastBank.Infrastructure.DTOs;
using FastBank.Services;
using Infrastructure.Context;

namespace FastBank
{
    public static class MenuOptions
    {
        static public Customer? ActiveCustumer = null;

        static bool inProgress = true;
        static public void ShowMainMenu()
        {
            FastBankDbContext db = new FastBankDbContext();
            var repo = new Repository(db);

            while (inProgress)
            {
                Console.Clear();
                if (ActiveCustumer == null)
                {
                    Console.WriteLine("Please choose your action:");
                    Console.WriteLine("1: For login. 2: For registration. 0: for exit");
                    int action = Convert.ToInt32(Console.ReadLine());
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
                    Console.Clear();
                    Console.WriteLine($"Welkom to FastBank as {ActiveCustumer.Role}");
                    switch (ActiveCustumer.Role)
                    {
                        case Roles.None:
                            Console.WriteLine($"Welkom to FastBank! You have any right in the system, please speak with administration");
                            Console.ReadKey();
                            ActiveCustumer = null;
                            break;
                        case Roles.Accunter:
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
                            OpenCustomerMenu();
                            break;
                    }
                   
                }
            }
        }
        static public void Login()
        {
            ICustomerService customerService = new CustomerService();

            Console.Clear();

            Console.WriteLine("Please input login(email):");
            var currentEmail = Console.ReadLine();

            if (customerService.CheckLoginUserName(currentEmail))
            {
                var passwordtries = 0;
                while (passwordtries < 3)
                {
                    Console.WriteLine("Please input password:");
                    var inputPassword = Console.ReadLine();
                    var loginCustomer = customerService.Login(currentEmail, inputPassword);
                    if (loginCustomer != null)
                    {
                        Console.WriteLine("Authorized");
                        ActiveCustumer = loginCustomer;
                        break;
                    }
                    else
                    {
                        passwordtries++;
                    }
                }
                if (passwordtries >= 3)
                {
                    Console.WriteLine("You try to login with wrong password 3 times! Press any key to continue...");
                    Console.ReadKey(true);
                    MenuOptions.ShowMainMenu();
                }
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

            customerService.Add(name, email, birthday, password, role);

            MenuOptions.ShowMainMenu();
        }

        static public void OpenCustomerMenu()
        {
            if (ActiveCustumer == null)
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
                            ActiveCustumer = null;
                            break;
                        }
                }
            }
        }
    }
}
