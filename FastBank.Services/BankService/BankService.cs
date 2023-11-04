using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.Repository;

namespace FastBank.Services.BankService
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _repoBank;
        public BankService()
        {
            _repoBank = new BankRepository();
        }
        public void CapitalReplenishment(User user)
        {
            //Here we should
            //1.Validate user rights
            //2.Show current bank capital amount
            //3.Ask about amount to be replenished
            //4 Confirm the operation
            //5.Create transaction about action

            if (user.Role != Roles.Banker)
            {
                Console.WriteLine("You have not access to this operation. Press any key for exit!");
                Console.ReadKey();
                return;
            }

            var bank = Get();

            if (bank == null)
            {
                Console.WriteLine("Bank is not ready. Press any key for exit!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine($"Fast bank have capital amount:{bank?.CapitalAmount}");

            decimal capitalAmountToReplenish;
            do
            {
                Console.WriteLine("Please write the amount to replenish the capital (type 'q' for exit):");
                Console.Write("Replenish the capital amount:");
                var inputcapitalAmountToReplenish = Console.ReadLine();
                if (inputcapitalAmountToReplenish == "q")
                    return;

                if (!decimal.TryParse(inputcapitalAmountToReplenish, out capitalAmountToReplenish) || capitalAmountToReplenish <= 0)
                {
                    Console.WriteLine("Please input correct amount to replenishment (press any key to continue...)");
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
