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
    }
}
