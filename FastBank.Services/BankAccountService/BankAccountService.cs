using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;

namespace FastBank.Services.BankAccountService
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository bankAccountRepository;

        public BankAccountService()
        {
            bankAccountRepository = new BankAccountRepository();
        }

        public BankAccount? GetBankAccount(Customer customer)
        {
            return bankAccountRepository.GetBankAccountByCustomer(customer);
        }

        public BankAccount? Add(Customer customer, decimal amount)
        {
            var bankAccount = GetBankAccount(customer);

            if (bankAccount == null && customer.Role == Roles.Customer)
            {
                bankAccount = new BankAccount(Guid.NewGuid(), customer, amount);
                bankAccountRepository.Add(bankAccount);
            }

            return bankAccount;
        }

        public void DepositAmount(Customer customer, ref BankAccount? customerBankAccount)
        {
            decimal depositAmount;
            do
            {
                Console.WriteLine("Please write the deposit amount (type 'q' for exit):");
                Console.Write("Deposit amount: ");
                var inputDepositAmount = Console.ReadLine();
                if (inputDepositAmount == "q")
                    return;

                if (!decimal.TryParse(inputDepositAmount, out depositAmount) || depositAmount <= 0)
                {
                    Console.WriteLine("Please input correct amount to deposit (press any key to continue...)");
                    var keyIsEnter = Console.ReadKey();
                    new MenuService().MoveToPreviousLine(keyIsEnter, 3);
                }
            }
            while (depositAmount <= 0);

            if (depositAmount > 0)
            {
                if (customerBankAccount == null)
                {
                    customerBankAccount = Add(customer, depositAmount);
                }
                else
                {
                    customerBankAccount.DepositAmount(depositAmount);
                    Update(customerBankAccount);
                }
            }
        }

        public void Update(BankAccount bankAccount)
        {
            bankAccountRepository.Update(bankAccount);
        }

        public void WithdrawAmount(Customer customer, BankAccount customerBankAccount)
        {
            decimal withdrawAmount;
            do
            {
                Console.WriteLine("Please write the withdraw amount:");
                Console.Write("Withdraw amount: ");
                var inputWithdrawAmount = Console.ReadLine();
                if (!decimal.TryParse(inputWithdrawAmount, out withdrawAmount) || withdrawAmount <= 0)
                {
                    Console.WriteLine("Plese input correct ammount to withdraw (press any key to continue...)");
                    var keyIsEnter = Console.ReadKey();
                    new MenuService().MoveToPreviousLine(keyIsEnter, 2);
                }
            } 
            while (withdrawAmount <=0);
            
            if (withdrawAmount > 0)
            {
                bool hasEnoughFunds = (customerBankAccount.Amount - withdrawAmount) < 0;
                if (hasEnoughFunds)
                {
                    Console.WriteLine("You do not have enough funds to withdraw (press any key to continue...)");
                    Console.ReadKey();
                }
                else
                {
                    customerBankAccount.WithdrawAmount(withdrawAmount);
                    Update(customerBankAccount);
                }
            }
        }

        public void TransferAmountToFriend(Customer customer, BankAccount customerBankAccount, Customer friend, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
