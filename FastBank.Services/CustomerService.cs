using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;

namespace FastBank.Services
{
    public class CustomerService : ICustomerService
    {
        readonly private ICustomerRepository _customerRepo;

        public CustomerService()
        {
            _customerRepo = new CustomerRepository();
        }

        public List<string> ValidationErrors { get; private set; } = new List<string>();

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
            ValidatCustomer(customer);
            if (ValidationErrors.Any())
            {
                Console.WriteLine("Customer data is not valid:");
                foreach (var error in ValidationErrors)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine("Please try again!");
            }
            else
            {
                Add(customer);
            }

        }

        public void ValidatCustomer(Customer customer)
        {
            ValidationErrors.Clear();
            CustomerExist(customer);
            //TODO validate password 
            //TODO validate email
            //TODO validate age to be more then 18 and less then 100
            //TODO validate role
        }

        public void CustomerExist(Customer customer)
        {
            if (_customerRepo.GetAll().Any(c => c.Name == customer.Name))
            {
                ValidationErrors.Add($"Customer with name: {customer.Name} already exist");
            }
            if (_customerRepo.GetAll().Any(c => c.Email == customer.Email))
            {
                ValidationErrors.Add($"Customer with email: {customer.Email} already exist");
            }
        }
        public bool CheckLoginUserName(string? username)
        {
            ValidationErrors.Clear();
            var customer = _customerRepo.GetAll().FirstOrDefault(c => c.Email == username);
            if (customer == null)
            {
                ValidationErrors.Add($"Customer with username(email): {username} not exist");
            }
            if (ValidationErrors.Any())
            {
                foreach (var error in ValidationErrors)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine("Unauthorized");
                Console.WriteLine("Please try again!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();

                return false;
            }

            return true;
        }

        public bool Login(string username, string password)
        {
            ValidationErrors.Clear();
            var customer = _customerRepo.GetAll().FirstOrDefault(c => c.Email == username);
            if (customer == null)
            {
                ValidationErrors.Add($"Customer with name: {username} not exist");
            }
            else
            {
                if (customer.Password != password)
                {
                    ValidationErrors.Add($"Wrong password! Try again!");
                }
            }

            if (ValidationErrors.Any())
            {
                foreach (var error in ValidationErrors)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine("Please try again!");

                return false;
            }

            return true;
        }
    }
}
