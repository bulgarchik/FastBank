namespace FastBank.Domain
{
    public class BankAccount
    {
       public BankAccount(Guid bankAccountId, User customer, decimal amount) 
        {
            BankAccountId = bankAccountId;
            Customer = customer;
            Amount = amount;
        }

        public Guid BankAccountId { get; private set; }
        public User Customer { get; private set; }
        public decimal Amount { get; private set; }

        public void DepositAmount(decimal depositAmount)
        {
            Amount += depositAmount;
        }

        public void WithdrawAmount(decimal withdrawAmount)
        {
            Amount -= withdrawAmount;
        }
    }
}
