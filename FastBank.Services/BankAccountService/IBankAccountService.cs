using FastBank.Domain;

namespace FastBank.Services
{
    public interface IBankAccountService
    {
        public BankAccount? GetBankAccount(Customer customer);

        public BankAccount? Add(Customer customer, decimal amount);

        public void Update(BankAccount bankAccount);

        public void DepositAmount(Customer customer, ref BankAccount? customerBankAccount);

        public void WithdrawAmount(BankAccount customerBankAccount);

        public void TransferMoneyToFriend(BankAccount customerBankAccount, Dictionary<int, User> friends, bool transferOrder = false);

        public void TransferMoneyToFriendMenu(BankAccount customerBankAccount);

        public void ConfirmTransactionOrder(TransactionOrder transactionOrder);
    }
}
