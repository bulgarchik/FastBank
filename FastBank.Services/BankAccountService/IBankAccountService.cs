using FastBank.Domain;

namespace FastBank.Services.BankAccountService
{
    public interface IBankAccountService
    {
        public BankAccount? GetBankAccount(Customer customer);

        public void Add(Customer customer, decimal amount);

        public void Update(BankAccount bankAccount);
    }
}
