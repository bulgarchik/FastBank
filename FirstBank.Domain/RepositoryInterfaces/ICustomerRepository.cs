namespace FastBank.Domain.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAll();

        public void Add(Customer customer);
    }
}
