using System.ComponentModel.DataAnnotations;

namespace FastBank.Domain
{
    public class Transaction
    {
        public Transaction(
            Guid transactionId,
            DateTime createdDate,
            User createdByUser,
            decimal amount,
            string userNameInitial,
            Bank? bank,
            BankAccount? bankAccount,
            TransactionType transactionType)
        {
            TransactionId = transactionId;
            CreatedDate = createdDate;
            CreatedByUser = createdByUser;
            Amount = amount;
            UserNameInitial = userNameInitial;
            Bank = bank;
            BankAccount = bankAccount;
            TransactionType = transactionType;
        }

        public Transaction(
            User createdByUser,
            decimal amount,
            Bank? bank,
            BankAccount? bankAccount,
            TransactionType transactionType)
        {
            TransactionId = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
            CreatedByUser = createdByUser;
            Amount = amount;
            UserNameInitial = createdByUser.Name.Trim().Substring(0, 2).ToUpper();
            Bank = bank;
            BankAccount = bankAccount;
            TransactionType = transactionType;
        }

        public Guid TransactionId { get; private set; }
        public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
        public User CreatedByUser { get; private set; }
        public decimal Amount { get; private set; }
        public string UserNameInitial { get; private set; }
        public Bank? Bank { get; private set; }
        public BankAccount? BankAccount { get; private set; }
        public TransactionType TransactionType { get; private set; }
    }
}

public enum TransactionType
{
    [Display(Name = "Bank Transaction")]
    BankTransaction,

    [Display(Name = "Bank Account Transaction")]
    BankAccountTransaction,

    [Display(Name = "Internal Transfer")]
    InternalTransfer,
}

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumType)
    {
        var displayAttribute = GetDisplayAttribute(enumType);
        return displayAttribute?.Name ?? enumType.ToString();
    }

    private static DisplayAttribute GetDisplayAttribute(Enum enumType)
    {
        var field = enumType.GetType().GetField(enumType.ToString());
        return (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));
    }
}
