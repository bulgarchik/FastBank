using FastBank.Domain;

namespace FastBank.Services.BankService
{
    public interface IBankService
    {
        public void Add(Bank bank);

        public Bank? Get();
    }
}
