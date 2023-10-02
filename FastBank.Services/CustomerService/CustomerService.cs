using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;

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

        public void Add(string name, string email, DateTime birthday, string password, Roles role)
        {
            var customer = new Customer(Guid.NewGuid(), name, email, birthday, password, role);
            var validationErrors = ValidatеCustomer(customer);
            if (validationErrors.Any())
            {
                Console.WriteLine("Customer data is not valid:");
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
            //TODO validate email
            //TODO validate age to be more then 18 and less then 100
            //TODO validate role
            return validationErrors;
        }

        public List<string> CustomerExist(Customer customer, List<string> validationErrors)
        {
            var customers = _customerRepo.GetAll(); //TODO use IQueryable

            if (customers.Any(c => c.Email == customer.Email))
            {
                validationErrors.Add($"Customer with email: {customer.Email} already exist");
            }
            return validationErrors;
        }
        public List<string> CheckLoginUserName(string? username) //TODO rename to email
        {
            var validationErrors = new List<string>();
            var customer = _customerRepo.GetAll().FirstOrDefault(c => c.Email == username); //TODO modify 
            if (customer == null)
            {
                validationErrors.Add($"Customer with username(email): {username} not exist");
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

        public Customer? Login(string username, string password)
        {
            var validationErrors = new List<string>();
            var customer = _customerRepo.GetAll().FirstOrDefault(c => c.Email == username);
            if (customer == null)
            {
                validationErrors.Add($"Customer with name: {username} not exist");
            }
            else
            {
                if (customer.Password != password)
                {
                    validationErrors.Add($"Wrong password! Try again!");
                    customer = null;
                }
            }

            if (validationErrors.Any())
            {
                foreach (var error in validationErrors)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine("Please try again!");
            }

            return customer;
        }
    }
}
