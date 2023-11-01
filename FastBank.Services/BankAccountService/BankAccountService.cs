using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;
using System.Text.RegularExpressions;

namespace FastBank.Services.BankAccountService
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository bankAccountRepository;
        private readonly IMenuService _menuService;
        private readonly IUserService _userService;

        public BankAccountService()
        {
            bankAccountRepository = new BankAccountRepository();
            _menuService = new MenuService();
            _userService = new UserService();
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
                Console.WriteLine("Please write the withdraw amount (type 'q' for exit):");
                Console.Write("Withdraw amount: ");
                var inputWithdrawAmount = Console.ReadLine();
                if (inputWithdrawAmount == "q")
                    return;
                if (!decimal.TryParse(inputWithdrawAmount, out withdrawAmount) || withdrawAmount <= 0)
                {
                    Console.WriteLine("Plese input correct ammount to withdraw (press any key to continue...)");
                    var keyIsEnter = Console.ReadKey();
                    new MenuService().MoveToPreviousLine(keyIsEnter, 3);
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

        public void TransferMoneyToFriend(Customer customer, BankAccount customerBankAccount, Customer friend, decimal amount)
        {
            //1 Open new screen for money transfer
            //2 Show the list of friends
            //3 Show menu options: 1. Add friend; 2 Transfer money; 0 for exit
            //4 On money transfer write friend email or number
            //5 Write amount
            //6 Confirm with Y...If it is first tranfer to friend to confirm friend with Y 
            //7 Execute Withdraw for current user and execute deposit to friend user
            throw new NotImplementedException();
        }

        public void TransferMoneyToFriendMenu(Customer customer)
        {
            Console.Clear();
            _menuService.Logo();

            Dictionary<int,User> friends = new Dictionary<int,User>();
            var friendIndex = 0;
            var friendsList = _userService.GetUserFriends(customer);
            if (friendsList != null)
            {

                Console.WriteLine($"\nYou have {friendsList.Count} friend{(friendsList.Count>1?'s':string.Empty) }:\n");
                foreach (var friend in friendsList)
                {
                    friends.Add(++friendIndex, friend);
                    Console.WriteLine($"{friendIndex}. Name: {friend.Name}; email: {friend.Email}");
                }
            }

            var menuOptions = $"\nPlease choose your action: " +
                             $"\n  0: for exit";
            int action = _menuService.CommandRead(new Regex("^[0]{1}$"), menuOptions);

            switch (action)
            {
                case 0: return;
            }
        }
    }
}
