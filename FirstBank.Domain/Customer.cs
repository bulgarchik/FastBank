using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Password { get; set; }
        public Roles Role { get; set; }

        static public void CustomerRegistration()
        {
            Console.Clear();

            Customer customer = new Customer();
            customer.Id = new Guid(Guid.NewGuid().ToString());
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
        }
    }
}
