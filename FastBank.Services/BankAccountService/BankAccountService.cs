using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;

namespace FastBank.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository bankAccountRepository;
        private readonly IMenuService _menuService;
        private readonly IUserService _userService;
        private readonly ITransactionRepository _transactionRepo;
        private readonly IMessageService _messageService;

        public BankAccountService()
        {
            bankAccountRepository = new BankAccountRepository();
            _menuService = new MenuService();
            _userService = new UserService();
            _transactionRepo = new TransactionRepository();
            _messageService = new MessageService(this);
        }

        public BankAccount? GetBankAccount(Customer customer)
        {
            return bankAccountRepository.GetBankAccountByCustomer(customer);
        }

        public BankAccount? Add(Customer customer, decimal amount)
        {
            var bankAccount = GetBankAccount(customer);

            if (bankAccount == null && customer.Role == Role.Customer)
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

            if (customerBankAccount == null)
            {
                customerBankAccount = Add(customer, depositAmount);
            }
            else
            {
                customerBankAccount.DepositAmount(depositAmount);
                Update(customerBankAccount);
            }
            var transaction = new Transaction(customer, depositAmount, null, customerBankAccount, TransactionType.BankAccountTransaction);
            _transactionRepo.AddTransaction(transaction);
        }

        public void Update(BankAccount bankAccount)
        {
            bankAccountRepository.Update(bankAccount);
        }

        public void WithdrawAmount(BankAccount customerBankAccount)
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
                    Console.WriteLine("Please input correct amount to withdraw (press any key to continue...)");
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
                    var transaction = new Transaction(customerBankAccount.Customer, withdrawAmount * (-1), null, customerBankAccount, TransactionType.BankAccountTransaction);
                    _transactionRepo.AddTransaction(transaction);
                }
            }
        }

        public void TransferMoneyToFriend(BankAccount customerBankAccount, Dictionary<int, User> friends, bool transferOrder = false)
        {
            var inquiryMsg = $"Please enter your friend's email for the money transfer{(transferOrder ? " order" : string.Empty)}. (type \"quit\" for exit):";
            var emailTypeToInput = "Friend email: ";
            var emailFriend = _menuService.InputEmail(inquiryMsg, emailTypeToInput);

            if (emailFriend == "quit")
                return;

            var friend = friends.Where(f => f.Value.Email == emailFriend).Select(f => f.Value).FirstOrDefault();
            if (friend == null)
            {
                Console.WriteLine("This email is not in your friend list");
                Console.ReadKey(true);
                return;
            }

            decimal amountToTransfer;
            do
            {
                Console.WriteLine($"Please write the amount to {(transferOrder ? "order" : string.Empty)} transfer to friend (type 'q' for exit):");
                Console.Write("Transfer amount: ");
                var inputTransferAmount = Console.ReadLine();
                if (inputTransferAmount == "q")
                    return;
                if (!decimal.TryParse(inputTransferAmount, out amountToTransfer) || amountToTransfer <= 0)
                {
                    Console.WriteLine($"Please enter correct amount to{(transferOrder ? " order" : string.Empty)} transfer (press any key to continue...)");
                    var keyIsEnter = Console.ReadKey();
                    new MenuService().MoveToPreviousLine(keyIsEnter, 3);
                }
            }
            while (amountToTransfer <= 0);

            if (amountToTransfer > 0)
            {
                bool hasEnoughFunds = (customerBankAccount.Amount - amountToTransfer) > 0;
                if (!hasEnoughFunds)
                {
                    Console.WriteLine($"You do not have enough funds to{(transferOrder ? " order" : string.Empty)} transfer (press any key to continue...)");
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
                        Console.WriteLine($"Please confirm with Y{(transferOrder ? " order" : string.Empty)} transfer  of {amountToTransfer} to {friend.Name} or press any other key to cancel...");
                        var confirmKey = Console.ReadKey();
                        if (confirmKey.KeyChar == 'Y')
                        {
                            if (transferOrder)
                            {
                                var transactionOrder = new TransactionOrder(
                                                                    Guid.NewGuid(),
                                                                    DateTime.UtcNow,
                                                                    TransactionType.InternalTransfer,
                                                                    customerBankAccount,
                                                                    friendAccount,
                                                                    null,
                                                                    customerBankAccount.Customer,
                                                                    amountToTransfer);

                                _transactionRepo.AddTransactionOrder(transactionOrder);

                                _messageService.AddMessage(subject: "Transfer order",
                                                           text: $"Please execute transfer from {transactionOrder?.FromBankAccount?.Customer.Name}" +
                                                           $"(email: {transactionOrder?.FromBankAccount?.Customer.Email}) to " +
                                                           $"{transactionOrder?.ToBankAccount?.Customer.Name}" +
                                                           $"(email: {transactionOrder?.ToBankAccount?.Customer.Email}) for " +
                                                           $"amount {transactionOrder?.Amount}",
                                                           MessageStatus.Sent,
                                                           MessageType.InqueryForOrderTransfer,
                                                           customerBankAccount.Customer,
                                                           null,
                                                           Role.CustomerService,
                                                           null,
                                                           null,
                                                           transactionOrder);
                                _menuService.OperationCompleteScreen();
                            }
                            else
                            {
                                friendAccount.DepositAmount(amountToTransfer);
                                Update(friendAccount);
                                var transactionDeposit = new Transaction(friendAccount.Customer, amountToTransfer, null, friendAccount, TransactionType.BankAccountTransaction);
                                _transactionRepo.AddTransaction(transactionDeposit);

                                customerBankAccount.WithdrawAmount(amountToTransfer);
                                Update(customerBankAccount);
                                var transactionWithdraw = new Transaction(customerBankAccount.Customer, amountToTransfer * (-1), null, customerBankAccount, TransactionType.BankAccountTransaction);
                                _transactionRepo.AddTransaction(transactionWithdraw);

                                _menuService.OperationCompleteScreen();
                            }
                        }

                    }
                }
            }
        }

        public void TransferMoneyToFriendMenu(BankAccount customerBankAccount)
        {
            Console.Clear();
            _menuService.ShowLogo();

            Dictionary<int, User> friends = new Dictionary<int, User>();
            var friendIndex = 0;
            var friendsList = _userService.GetUserFriends(customerBankAccount.Customer);
            if (friendsList != null)
            {
                var friendListCount = friendsList.Count;
                Console.WriteLine($"\nYou have {friendsList.Count} friend{(friendListCount > 1 || friendListCount == 0 ? 's' : string.Empty)}:\n");
                foreach (var friend in friendsList)
                {
                    friends.Add(++friendIndex, friend);
                    Console.WriteLine($"{friendIndex}. Name: {friend.Name}; Email: {friend.Email}");
                }
            }

            Console.WriteLine($"\nYou bank amount: {customerBankAccount.Amount:0.00} ");

            var menuOptions = $"\nPlease choose your action: \n" +
                             $"\n 1: Add friend \n 2: Remove friend \n 3: Transfer money to friend \n 4: Request money transfer  \n 0: Exit";
            var commandsCount = 5;
            int action = _menuService.CommandRead(commandsCount, menuOptions);

            switch (action)
            {
                case 1:
                    {
                        _userService.AddFriend(customerBankAccount.Customer, friendsList ?? new List<User>());
                        break;
                    };

                case 2:
                    {
                        _userService.RemoveFriend(customerBankAccount.Customer, friendsList ?? new List<User>());
                        break;
                    };
                case 3:
                    {
                        TransferMoneyToFriend(customerBankAccount, friends);
                        break;
                    }
                case 4:
                    {
                        TransferMoneyToFriend(customerBankAccount, friends, transferOrder: true);
                        break;
                    }
                case 0: return;
            }

            TransferMoneyToFriendMenu(customerBankAccount);
        }

        public void ConfirmTransactionOrder(TransactionOrder transactionOrder)
        {
            transactionOrder?.FromBankAccount?.WithdrawAmount(transactionOrder.Amount);
            if (transactionOrder?.FromBankAccount != null)
            {
                Update(transactionOrder.FromBankAccount);

                _transactionRepo.AddTransaction(new Transaction(
                                                    transactionOrder.FromBankAccount.Customer,
                                                    transactionOrder.Amount * (-1),
                                                    null,
                                                    transactionOrder.FromBankAccount,
                                                    TransactionType.BankAccountTransaction));
            }
            if (transactionOrder?.ToBankAccount != null)
            {
                transactionOrder.ToBankAccount.DepositAmount(transactionOrder.Amount);
                Update(transactionOrder.ToBankAccount);

                _transactionRepo.AddTransaction(new Transaction(
                                                    transactionOrder.ToBankAccount.Customer,
                                                    transactionOrder.Amount,
                                                    null,
                                                    transactionOrder.ToBankAccount,
                                                    TransactionType.BankAccountTransaction));
            }
        }
    }
}
