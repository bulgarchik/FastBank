namespace FastBank.Domain
{
    public class Message
    {
        public Message(
            Guid messageId,
            DateTime createdOn,
            User? sender,
            User? receiver,
            Role receiverRole,
            string text,
            string subject,
            Message? basedOnMessage,
            MessageStatus messageStatuses,
            MessageType messageType,
            Transaction? transaction = null,
            TransactionOrder? transactionOrder = null)
        {
            MessageId = messageId;
            CreatedOn = createdOn;
            Sender = sender;
            Receiver = receiver;
            ReceiverRole = receiverRole;
            Text = text;
            Subject = subject;
            BasedOnMessage = basedOnMessage;
            MessageStatus = messageStatuses;
            MessageType = messageType;
            Transaction = transaction;
            TransactionOrder = transactionOrder;
        }
        public Guid MessageId { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public User? Sender { get; private set; }
        public User? Receiver { get; private set; }
        public Role ReceiverRole { get; private set; }
        public string Text { get; private set; } = string.Empty;
        public string Subject { get; private set; } = string.Empty;
        public Message? BasedOnMessage { get; private set; }
        public string MessageOrderId { get; set; }
        public int MessageLevel { get; set; }
        public MessageStatus MessageStatus { get; private set; }
        public MessageType MessageType { get; private set; }
        public Transaction? Transaction { get; private set; }
        public TransactionOrder? TransactionOrder { get; private set; }
        public int Index { get; set; } = 0;

        public void UpdateMessageStatus(MessageStatus messageStatuses)
        {
            this.MessageStatus = messageStatuses;
        }
    }

    public enum MessageStatus
    {
        Sent,
        Delivered,
        Accepted,
        Replied
    }

    public enum MessageType
    {
        Inquiry,
        InquiryForOrderTransfer,
        CapitalReplenishment
    }
}
