namespace FastBank.Domain
{
    public class BankAccount
    {
       public BankAccount(Guid bankAccountId, Customer customer, decimal amount) 
        {
            BankAccountId = bankAccountId;
            Customer = customer;
            Amount = amount;
        }

        public Guid BankAccountId { get; private set; }

        public Customer Customer { get; private set; }

        public decimal Amount { get; private set; }

        public void DepositAmount(decimal depositAmount)
        {
            Amount += depositAmount;
        }
    }
}
