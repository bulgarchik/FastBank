using FastBank.Domain;

namespace FastBank.Services.BankAccountService
{
    public interface IBankAccountService
    {
        public BankAccount? GetBankAccount(Customer customer);

        public BankAccount? Add(Customer customer, decimal amount);

        public void Update(BankAccount bankAccount);

        public void DepositAmount(Customer customer, ref BankAccount? customerBankAccount);

        public void WithdrawAmount(BankAccount customerBankAccount);

        public void TransferMoneyToFriend(BankAccount customerBankAccount, Dictionary<int, User> friends, bool transderOrder = false);

        public void TransferMoneyToFriendMenu(BankAccount customerBankAccount);
    }
}
