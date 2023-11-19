using FastBank.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.Infrastructure.DTOs
{
    [Table("TransactionOrders")]
    public class TransactionOrderDto
    {
        private TransactionOrderDto() { }
        public TransactionOrderDto(TransactionOrder transactionOrder)
        {
            TransactionOrderId = transactionOrder.TransactionOrderId;
            CreatedDate = transactionOrder.CreatedDate;
            TransactionType = transactionOrder.TransactionType;
            FromBankAccountId = transactionOrder?.FromBankAccount?.BankAccountId;
            ToBankAccountId = transactionOrder?.ToBankAccount?.BankAccountId;
            BankId = transactionOrder?.Bank?.Id;
            OrderedByUserId = transactionOrder?.OrderedByUser.Id;
            Amount = transactionOrder?.Amount ?? 0;
        }

        [Key]
        public Guid TransactionOrderId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public TransactionType TransactionType { get; private set; }

        public Guid? FromBankAccountId { get; private set; }
        [ForeignKey(nameof(FromBankAccountId))]
        public BankAccountDTO? FromBankAccount { get; private set; }
        
        public Guid? ToBankAccountId {  get; private set; }
        [ForeignKey(nameof(ToBankAccountId))]
        public BankAccountDTO? ToBankAccount { get; private set; }
        
        public Guid? BankId { get; private set; }
        [ForeignKey(nameof(BankId))]
        public BankDTO? Bank { get; private set; }
        
        public Guid? OrderedByUserId { get; private set; }
        [ForeignKey(nameof(OrderedByUserId))]
        public UserDTO OrderedByUser { get; private set; } = null!;
        public decimal Amount { get; private set; }

        public TransactionOrder ToDomainObj()
        {
            return new TransactionOrder(
                TransactionOrderId,
                CreatedDate,
                TransactionType,
                FromBankAccount?.ToDomainObj(),
                ToBankAccount?.ToDomainObj(),
                Bank?.ToDomainObj(),
                OrderedByUser?.ToDomainObj(),
                Amount);
        }
    }
}
