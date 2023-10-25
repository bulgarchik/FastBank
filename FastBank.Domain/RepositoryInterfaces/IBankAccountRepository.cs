namespace FastBank.Domain.RepositoryInterfaces
{
    public interface IBankAccountRepository
    {
        public BankAccount? GetBankAccountByCustomer(User customer);

        public void Add(BankAccount bankAccount);

        public void Update(BankAccount bankAccount);
    }
}
