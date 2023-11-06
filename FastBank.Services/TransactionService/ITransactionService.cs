using FastBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Services.TransactionService
{
    public interface ITransactionService
    {
        public Transaction AddTranscation(
            User createdByUser,
            decimal amount,
            Bank? bank,
            BankAccount? bankAccount,
            TransactionType transactionType);
    }
}
