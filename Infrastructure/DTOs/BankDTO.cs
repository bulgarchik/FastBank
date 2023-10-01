using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Infrastructure.DTOs
{
    [Table("Banks")]
    public class BankDTO
    {
        private BankDTO() { }

        public BankDTO(int capitalAmount)
        {
            BankId = Guid.NewGuid();
            CapitalAmount = capitalAmount;

        }
        [Key]
        public Guid BankId { get; private set; }
        public int CapitalAmount { get; private set; }
    }
}
