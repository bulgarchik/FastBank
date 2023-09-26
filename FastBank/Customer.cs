using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Password { get; set; }
        public Roles Role { get; set; }

        static public List<Customer> Customers = new List<Customer>()
        {
            new Customer { Id = 1, Name = "Alex Alex", Birthday = new DateTime(1983,10,10), Email = "shevrikuko.alex@gmail.com",Password = "123" },
            new Customer { Id = 2, Name = "Angel Angelov", Birthday = new DateTime(1993,10,10), Email = "Angel.angelov@gmail.com",Password = "123" }
        };

        static public void CustomerRegistration() 
        {
            Console.Clear();
            
            Customer customer = new Customer();
            customer.Id = Customer.Customers.Count+1;
            customer.Role = Roles.Customer;

            Console.WriteLine("Please input registration data about you:");
            Console.WriteLine("Please input you name:");
            customer.Name = Console.ReadLine();
            Console.WriteLine("Please input you email:");
            customer.Email = Console.ReadLine();
            Console.WriteLine("Please input you Birthday:");
            customer.Birthday = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Please input you password:");
            customer.Password = Console.ReadLine();
            Customers.Add(customer);

            MenuOptions.ShowMainMenu();
        }
    }
}
