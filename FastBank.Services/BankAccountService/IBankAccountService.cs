using FastBank.Domain;

namespace FastBank.Services.BankAccountService
{
    public interface IBankAccountService
    {
        public BankAccount? GetBankAccount(User customer);

        public void Add(User customer, decimal amount);

        public void Update(BankAccount bankAccount);

        public void DepositAmount(User customer, BankAccount customerBankAccount);

        public void WithdrawAmount(User customer, BankAccount customerBankAccount);

        public void TransferAmountToFriend(User customer, BankAccount customerBankAccount, User friend, decimal amount);
    }
}
