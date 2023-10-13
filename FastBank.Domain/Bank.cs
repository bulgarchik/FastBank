namespace FastBank.Domain
{
    public class Bank
    {
        public Bank(Guid id, decimal capitalAmount)
        {
            Id = id;
            CapitalAmount = capitalAmount;
        }

        public Guid Id { get; private set; }
        public decimal CapitalAmount { get; private set; }
    }
}
