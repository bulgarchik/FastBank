using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (GetBankAccount(customer) == null)
            {
                BankAccount bankAccount = new BankAccount(Guid.NewGuid(), customer, amount);
                bankAccountRepository.Add(bankAccount);
            }
        }

        public void DepositAmount(Customer customer, BankAccount customerBankAccount)
        {
            Console.Write("Please write the deposit amount:");
            var inputDepositAmount = Console.ReadLine();
            decimal depositAmount;
            while (!decimal.TryParse(inputDepositAmount, out depositAmount) && depositAmount < 0)
            {
                if (depositAmount < 0)
                {
                    Console.WriteLine("Please input an amount to deposit more than 0");
                }
                else if (depositAmount == 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Plese input correct ammount to deposit");
                }
                Console.ReadKey();

                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
                inputDepositAmount = Console.ReadLine();
            }

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
    }
}
