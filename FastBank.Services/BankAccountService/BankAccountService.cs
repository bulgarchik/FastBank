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
            while (withdrawAmount <= 0);

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

        public void TransferMoneyToFriend(Customer customer, BankAccount customerBankAccount, Dictionary<int, User> friends)
        {
            var inquiryMsg = "Please input friend's email to transfer money. (type \"quit\" for exit):";
            var emailTypeToInput = "Friend email:";
            var emailFriend = _menuService.InputEmail(inquiryMsg, emailTypeToInput);

            if (emailFriend == "quit")
                return;

            var friend = friends.Where(f => f.Value.Email == emailFriend).Select(f => f.Value).FirstOrDefault();

            if (friend == null)
                Console.WriteLine("This email is not in your friendlist");
                Console.ReadKey(true);
                return;

            decimal amountToTransfer;
            do
            {
                Console.WriteLine("Please write the amount for transfer to friend (type 'q' for exit):");
                Console.Write("Transfer amount: ");
                var inputTransferAmount = Console.ReadLine();
                if (inputTransferAmount == "q")
                    return;
                if (!decimal.TryParse(inputTransferAmount, out amountToTransfer) || amountToTransfer <= 0)
                {
                    Console.WriteLine("Plese input correct ammount to transfer (press any key to continue...)");
                    var keyIsEnter = Console.ReadKey();
                    new MenuService().MoveToPreviousLine(keyIsEnter, 3);
                }
            }
            while (amountToTransfer <= 0);

            if (amountToTransfer > 0)
            {
                bool hasEnoughFunds = (customerBankAccount.Amount - amountToTransfer) < 0;
                if (!hasEnoughFunds)
                {
                    Console.WriteLine("You do not have enough funds to transfer (press any key to continue...)");
                    Console.ReadKey();
                }
                else
                {
                    var friendAccount = bankAccountRepository.GetBankAccountByCustomer(new Customer(friend));
                    if (friendAccount == null)
                    {
                        Console.WriteLine("You friend has not bank account. Press any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine($"Please confirm with Y transfer of {amountToTransfer} to {friend.Name} or press any other key to cancel...");
                        var confirmKey = Console.ReadKey();
                        if (confirmKey.KeyChar == 'Y')
                        {
                            friendAccount.DepositAmount(amountToTransfer);
                            Update(friendAccount);
                            customerBankAccount.WithdrawAmount(amountToTransfer);
                            Update(customerBankAccount);
                        }
                    }
                }
            }
        }

        public void TransferMoneyToFriendMenu(Customer customer, BankAccount customerBankAccount)
        {
            Console.Clear();
            _menuService.ShowLogo();

            Dictionary<int, User> friends = new Dictionary<int, User>();
            var friendIndex = 0;
            var friendsList = _userService.GetUserFriends(customer);
            if (friendsList != null)
            {

                Console.WriteLine($"\nYou have {friendsList.Count} friend{(friendsList.Count > 1 ? 's' : string.Empty)}:\n");
                foreach (var friend in friendsList)
                {
                    friends.Add(++friendIndex, friend);
                    Console.WriteLine($"{friendIndex}. Name: {friend.Name}; email: {friend.Email}");
                }
            }

            var menuOptions = $"\nPlease choose your action: " +
                             $"\n1: Add friend. 2. Remove friend. 3. Transfer money to friend  0: for exit";
            int action = _menuService.CommandRead(4, menuOptions);

            switch (action)
            {
                case 1:
                    {
                        _userService.AddFriend(customer, friendsList ?? new List<User>());
                        break;
                    };

                case 2:
                    {
                        _userService.RemoveFriend(customer, friendsList ?? new List<User>());
                        break;
                    };
                case 3:
                    {
                        TransferMoneyToFriend(customer, customerBankAccount, friends);
                        break;
                    }

                case 0: return;
            }

            TransferMoneyToFriendMenu(customer, customerBankAccount);

        }
    }
}
