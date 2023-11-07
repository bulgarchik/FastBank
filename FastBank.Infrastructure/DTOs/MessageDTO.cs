using FastBank.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.Infrastructure.DTOs
{
    [Table("Messages")]
    public class MessageDTO
    {
        private MessageDTO() { }

        public MessageDTO(Message message)
        {
            MessageId = message.MessageId;
            SenderId = message.Sender.Id;
            ReceiverId = message.Receiver?.Id;
            ReceiverRole = message.ReceiverRole;
            Text = message.Text;
            Subject = message.Subject;
            BasedOnMessageId = message.BasedOnMessage?.MessageId;
            MessageStatus = message.MessageStatus;
            Type = message.MessageType;
            TransactionId = message?.Transaction?.TransactionId;
            TransactionOrderId = message?.TransactionOrder?.TransactionOrderId;
        }

        [Key]
        public Guid MessageId { get; private set; }
        public Guid SenderId { get; private set; }
        [ForeignKey(nameof(SenderId))]
        public UserDTO Sender { get; private set; }
        public Guid? ReceiverId { get; private set; }
        [ForeignKey(nameof(ReceiverId))]
        public UserDTO? Receiver { get; private set; } = null;
        public Role ReceiverRole { get; private set; }
        public string Text { get; private set; } = string.Empty;
        public string Subject { get; private set; } = string.Empty;
        public Guid? BasedOnMessageId { get; private set; }
        [ForeignKey(nameof(BasedOnMessageId))]
        public MessageDTO? BasedOnMessage { get; private set; }
        public MessageStatus MessageStatus { get; private set; }
        public MessageType Type { get; private set; }
        public Guid? TransactionId { get; private set; }
        [ForeignKey(nameof(TransactionId))]
        public TransactionDTO? Transaction { get; private set; }
        public Guid? TransactionOrderId { get; private set; }
        [ForeignKey(nameof(TransactionOrderId))]
        public TransactionOrderDto? TransactionOrder { get; private set; }

        public void UpdateMessageStatus(MessageStatus messageStatuses)
        {
            this.MessageStatus = messageStatuses;
        }
        public Message? ToDomainObj(int index)  
        {
            return new Message(
                MessageId,
                Sender?.ToDomainObj(),
                Receiver?.ToDomainObj(),
                ReceiverRole,
                Text,
                Subject,
                BasedOnMessage?.ToDomainObj(index),
                MessageStatus,
                Type,
                Transaction?.ToDomainObj(),
                TransactionOrder?.ToDomainObj(),
                index);
        }
    }
}
