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

        public void TransferMoneyToFriendMenu(Customer customer);

        public void TransferMoneyToFriend(Customer customer, BankAccount customerBankAccount, Customer friend, decimal amount);
    }
}
