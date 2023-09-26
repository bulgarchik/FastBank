using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank
{
    public static class MenuOptions
    {
        static bool inProgress = true;
        static public void ShowMainMenu()
        {
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
                            Customer.CustomerRegistration();
                            break;
                        }
                    case 0:
                        {
                            inProgress = false;
                            foreach (var custumer in Customer.Customers)
                            {
                                Console.WriteLine(custumer.Name);
                            }
                            break;

                        }
                }
            }
        }
        static public void Login(bool wrongEmail = false)
        {
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
            var customer = Customer.Customers.FirstOrDefault(a => a.Email == currentEmail);
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
    }
}
