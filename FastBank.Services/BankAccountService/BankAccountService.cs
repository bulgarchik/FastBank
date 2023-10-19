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

        public void Add(Customer customer, decimal amount)
        {
            var existBankAccount = GetBankAccount(customer);

            if (existBankAccount == null)
            {
                BankAccount bankAccount = new BankAccount(Guid.NewGuid(), customer, amount);
                bankAccountRepository.Add(bankAccount);
            }
        }

        public void DepositAmount(Customer customer, BankAccount customerBankAccount)
        {
            decimal depositAmount;
            do
            {
                Console.Write("Please write the deposit amount (type 'q' for exit):");
                var inputDepositAmount = Console.ReadLine();
                if (inputDepositAmount == "q")
                    return;

                if (!decimal.TryParse(inputDepositAmount, out depositAmount) || depositAmount <= 0)
                {
                    Console.WriteLine("Please input correct amount to deposit (press any key to continue...)");
                    Console.ReadKey();
                    new MenuService().MoveToPreviousLine(2);
                }
            }
            while (depositAmount <= 0);

            if (depositAmount > 0)
            {
                if (customerBankAccount == null)
                {
                    Add(customer, depositAmount);
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
                Console.Write("Please write the withdraw amount:");
                var inputWithdrawAmount = Console.ReadLine();
                if (!decimal.TryParse(inputWithdrawAmount, out withdrawAmount) || withdrawAmount < 0)
                {
                    Console.WriteLine("Plese input correct ammount to withdraw (press any key to continue...)");
                    Console.ReadKey();
                    new MenuService().MoveToPreviousLine(2);
                }
            } 
            while (withdrawAmount < 0);
            
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
    }
}
