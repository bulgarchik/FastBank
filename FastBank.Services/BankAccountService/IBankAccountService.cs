using FastBank.Domain;

namespace FastBank.Services.BankAccountService
{
    public interface IBankAccountService
    {
        public BankAccount? GetBankAccount(Customer customer);

        public BankAccount? Add(Customer customer, decimal amount);

        public void Update(BankAccount bankAccount);

        public void DepositAmount(Customer customer, ref BankAccount? customerBankAccount);

        public void WithdrawAmount(Customer customer, BankAccount customerBankAccount);

        public void TransferMoneyToFriend(Customer customer, BankAccount customerBankAccount, Dictionary<int, User> friends);

        public void TransferMoneyToFriendMenu(Customer customer, BankAccount customerBankAccount);
    }
}
