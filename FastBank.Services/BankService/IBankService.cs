using FastBank.Domain;

namespace FastBank.Services.BankService
{
    public interface IBankService
    {
        public Bank? Get();

        public void CapitalReplenishment(User user);
    }
}
