namespace FastBank.Services
{
    public interface ICustomerService
    {
        public List<Customer> GetAll();

        public void Add(Customer customer);

        public void Add(string name, string email, DateTime birthday, string password, Roles role, bool inActive);

        public List<string> CheckLoginUserName(string username);

        public Customer? Login(string username, string password);
    }
}
