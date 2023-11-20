using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;

namespace FastBank.Services.BankService
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _repoBank;
        private readonly ITransactionRepository _transactionRepo;
        private readonly IMessageService _messageService;
        private readonly IMenuService _menuService;

        public BankService()
        {
            _repoBank = new BankRepository();
            _transactionRepo = new TransactionRepository();
            _messageService = new MessageService(_bankAccountService: null);
            _menuService = new MenuService();
        }

        public void CapitalReplenishment(User user)
        {
            //Here we should
            //1.Validate user rights
            //2.Show current bank capital amount
            //3.Ask about amount to be replenished
            //4 Confirm the operation
            //5.Create transaction about action

            if (user.Role != Role.Banker)
            {
                Console.WriteLine("You have not access to this operation. Press any key for exit!");
                Console.ReadKey();
                return;
            }

            var bank = Get();

            if (bank == null)
            {
                Console.WriteLine("Bank isn't available. Press any key for exit!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine($"Current capital of Fast Bank :{bank?.CapitalAmount}");

            decimal capitalAmountToReplenish;
            do
            {
                Console.WriteLine("Please write the amount to replenish the capital (type 'q' for exit):");
                Console.Write("Replenish the capital amount:");
                var inputCapitalAmountToReplenish = Console.ReadLine();
                if (inputCapitalAmountToReplenish == "q")
                    return;

                if (!decimal.TryParse(inputCapitalAmountToReplenish, out capitalAmountToReplenish) || capitalAmountToReplenish <= 0)
                {
                    Console.WriteLine("Please input correct repenishment amount (press any key to continue...)");
                    var keyIsEnter = Console.ReadKey();
                    new MenuService().MoveToPreviousLine(keyIsEnter, 3);
                }
            }
            while (capitalAmountToReplenish <= 0);

            Console.WriteLine($"Please confirm replenishment of bank capital with {capitalAmountToReplenish} with Y or press any key to cancel...");

            var confirmKey = Console.ReadKey();
            if (confirmKey.KeyChar == 'Y' && bank != null)
            {
                _repoBank.ReplenishCapital(user, bank, capitalAmountToReplenish);
                var transaction = new Transaction(user, capitalAmountToReplenish, bank, null, TransactionType.BankTransaction);
                _transactionRepo.AddTransaction(transaction);
                var subjectMessage = $"Capital replenishment.";
                var textMessage = $"Transaction date: {transaction.CreatedDate}" +
                                  $"\nResponsible for replenishment: {transaction.CreatedByUser.Name} ({transaction.UserNameInitial})" +
                                  $"\nBalance before transaction: {bank.CapitalAmount}" +
                                  $"\nReplenishment amount: {capitalAmountToReplenish}" +
                                  $"\nBalance after transaction: {bank.CapitalAmount + capitalAmountToReplenish}";

                _messageService.AddMessage(subject: subjectMessage,
                                           text: textMessage,
                                           status: MessageStatus.Sent,
                                           type: MessageType.CapitalReplenishment,
                                           sender: user,
                                           receiver: null,
                                           receiverRole: Role.Manager,
                                           basedOnMessage: null,
                                           transaction: transaction,
                                           transactionOrder: null);

                _menuService.OperationCompleteScreen();
            }
            return;
        }

        public Bank? Get()
        {
            var bank = _repoBank.GetBank();

            return bank;
        }
    }
}
