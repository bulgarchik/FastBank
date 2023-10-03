namespace FastBank.Domain.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAll();

        public Customer? GetByEmail(string email);

        public void Add(Customer customer);
    }
}
