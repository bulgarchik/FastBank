using FastBank.Infrastructure;
using FastBank.Services;
using Infrastructure.Context;

namespace FastBank
{
    public static class MenuOptions
    {

        static bool inProgress = true;
        static public void ShowMainMenu()
        {
            FastBankDbContext db = new FastBankDbContext();
            var repo = new Repository(db);

            while (inProgress)
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
        }
        static public void Login(bool wrongEmail = false)
        {
            FastBankDbContext db = new FastBankDbContext();
            var repo = new Repository(db);

            Console.Clear();
            if (wrongEmail)
            {
                Console.WriteLine("User is not exist. Please input login(email) again:");
            }
            else
            {
                Console.WriteLine("Please input login(email):");
            }

            var currentEmail = Console.ReadLine();
            var customer = repo.Set<Customer>().FirstOrDefault(a => a.Email == currentEmail);
            if (customer == null)
            {
                Console.WriteLine("This user is not registered, pls try again!");

                MenuOptions.Login(true);
            }
            else
            {
                var passwordtries = 0;
                while (passwordtries < 3)
                {
                    if (PasswordCheck(customer) == false)
                    {
                        Console.WriteLine("Your password is wrong, please try again");
                        passwordtries++;
                    }
                    else
                    {
                        Console.WriteLine("Welcom to the Fast Bank System"); //TODO Open User menu
                        break;
                    };
                }
                if (passwordtries >= 3)
                {
                    Console.WriteLine("You try to login with wrong password 3 times!");
                    MenuOptions.ShowMainMenu();
                }

            }
        }
        static public bool PasswordCheck(Customer customer)
        {
            Console.WriteLine("Please input password:");
            var inputPassword = Console.ReadLine();
            return inputPassword == customer.Password;
        }
        
        static public void CustomerRegistration()
        {
            ICustomerService customerService= new CustomerService();
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
        
    }
}
