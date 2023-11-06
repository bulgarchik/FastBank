namespace FastBank.Domain.RepositoryInterfaces
{
    public interface IBankRepository
    {
        public void ReplenishCapital(User user, Bank bank, decimal amountToReplenish);

        public Bank? GetBank();
    }
}
