using FastBank.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.Infrastructure.DTOs
{
    [Table("Banks")]
    public class BankDTO
    {
        private BankDTO() { }

        public BankDTO(decimal capitalAmount)
        {
            BankId = Guid.NewGuid();
            CapitalAmount = capitalAmount;
        }

        [Key]
        public Guid BankId { get; private set; }
        public decimal CapitalAmount { get; private set; }
        public Bank ToDomainObj()
        {
            return new Bank(BankId, CapitalAmount);
        }

        public void ReplenishCapital(decimal capitalAmountToReplenish)
        {
            CapitalAmount = CapitalAmount + capitalAmountToReplenish;
        }
    }
}
