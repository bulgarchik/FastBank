﻿using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;
using FastBank.Services.MessageService;
using FastBank.Services.TransactionService;

namespace FastBank.Services.BankService
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _repoBank;
        private readonly ITransactionService _transactionService;
        private readonly IMessageService _messageService;

        public BankService()
        {
            _repoBank = new BankRepository();
            _transactionService = new TransactionService.TransactionService();
            _messageService = new MessageService.MessageService();
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
            Console.WriteLine($"Current capital of Fast Bank:{bank?.CapitalAmount}");

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
                var transaction = _transactionService.AddTranscation(user, capitalAmountToReplenish, bank, null, TransactionType.BankTransaction);
                var subjectMessage = $"Capital replenishment.";
                var textMessage = $"Transaction date: {transaction.CreatedDate}" +
                                  $"\nResponsible for replenishment: {transaction.CreatedByUser.Name} ({transaction.UserNameInitial})" +
                                  $"\nBalance before transaction: {bank.CapitalAmount}" +
                                  $"\nReplenishment amount: {capitalAmountToReplenish}" +
                                  $"\nBalance after transaction: {bank.CapitalAmount + capitalAmountToReplenish}";

                _messageService.AddMessage(subjectMessage,
                                           textMessage,
                                           MessageStatus.Sent,
                                           MessageType.CapitalReplenishment,
                                           user,
                                           null,
                                           Role.Manager,
                                           null,
                                           transaction);
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
