namespace FastBank.Domain.RepositoryInterfaces
{
    public interface IBankAccountRepository
    {
        public BankAccount? GetBankAccountByCustomer(Customer customer);
    }
}
