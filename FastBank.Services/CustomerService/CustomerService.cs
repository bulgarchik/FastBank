using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;
using System.Text.RegularExpressions;

namespace FastBank.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomerService()
        {
            _customerRepo = new CustomerRepository();
        }

        public List<Customer> GetAll()
        {
            return _customerRepo.GetAll();
        }

        public void Add(Customer customer)
        {
            _customerRepo.Add(customer);
        }

        public void Add(string name, string email, DateTime birthday, string password, Roles role, bool inactive)
        {
            var customer = new Customer(Guid.NewGuid(), name, email, birthday, password, role, inactive);
            var validationErrors = ValidatеCustomer(customer);
            if (validationErrors.Any())
            {
                Console.WriteLine("\nCustomer data is not valid:");
                foreach (var error in validationErrors)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine("Please try again!");
                Console.ReadKey();
            }
            else
            {
                Add(customer);
            }
        }

        public List<string> ValidatеCustomer(Customer customer)
        {
            var validationErrors = new List<string>();
            CustomerExist(customer, validationErrors);

            //TODO validate password 
            ValidateEmail(customer.Email, validationErrors);
            CustomerAgeIsValid(customer, validationErrors);
            //TODO validate role
            return validationErrors;
        }

        public List<string> ValidateEmail(string email, List<string> validationErrors)
        {
            var regEmailPattern = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            if (!regEmailPattern.IsMatch(email))
            {
                validationErrors.Add("You entered wrong email");
            }
            return validationErrors;
        }

        public List<string> CustomerExist(Customer customer, List<string> validationErrors)
        {
            if (_customerRepo.GetByEmail(customer.Email) != null)
            {
                validationErrors.Add($"Customer with email: {customer.Email} already exist");
            }
            return validationErrors;
        }

        public List<string> CustomerAgeIsValid(Customer customer, List<string> validationErrors)
        {
            var age = DateTime.Now.Year - customer.Birthday.Year;
            if (DateTime.Now.DayOfYear < customer.Birthday.DayOfYear)
            {
                age = age - 1;
            }

            if (age < 18)
            {
                validationErrors.Add($"The customer is underage (18)");
            }
            else if (age > 100)
            {
                validationErrors.Add($"The customer is over 100 years old");
            }
            return validationErrors;
        }

        public List<string> CheckLoginUserName(string email)
        {
            var validationErrors = new List<string>();
            var customer = _customerRepo.GetByEmail(email);
            if (customer == null)
            {
                validationErrors.Add($"Customer with username(email): {email} not exist");
                foreach (var error in validationErrors)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine("Unauthorized");
                Console.WriteLine("Please try again!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            return validationErrors;
        }

        public Customer? Login(string email, string password)
        {
            var customer = _customerRepo.GetByEmail(email);
            if (customer == null)
            {
                Console.WriteLine($"\nCustomer with name: {email} not exist. Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                if (customer.Inactive)
                {
                    Console.WriteLine($"Customer with name: {email} deactivated. Please contact Administration");
                    customer = null;
                    return customer;
                }
                var passwordtries = 0;
                var menuServie = new MenuService();
                while (passwordtries < 2)
                {
                    if (customer.Password != password)
                    {
                        Console.WriteLine($"Wrong password! Press any key to try again!");
                        Console.ReadKey();

                        new MenuService().MoveToPreviousLine(2);
                        passwordtries++;
                        Console.WriteLine("Please input password:");
                        
                        password = menuServie.PasswordStaredInput();
                        password = Console.ReadLine()??"";
                    }
                    else
                    {
                        return customer;
                    }
                }
                if (passwordtries == 2)
                {
                    Console.WriteLine("You try to login with wrong password 3 times! Press any key to continue...");
                    Console.ReadKey(true);
                    customer = null;
                }
            }
                        
            return customer;
        }
    }
}
