using FastBank.Domain;

namespace FastBank.Services.BankAccountService
{
    public interface IBankAccountService
    {
        public BankAccount GetBankAccount(Customer customer);
    }
}
