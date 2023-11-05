namespace FastBank.Domain
{
    public class Message
    {
        public Message(
            Guid messageId,
            User sender,
            User? receiver,
            Roles receiverRole,
            string text,
            string subject,
            Message? basedOnMessage,
            MessageStatuses messageStatuses,
            MessageType messageType,
            Transaction? transaction)
        {
            MessageId = messageId;
            Sender = sender;
            Receiver = receiver;
            ReceiverRole = receiverRole;
            Text = text;
            Subject = subject;
            BasedOnMessage = basedOnMessage;
            Status = messageStatuses;
            Type = messageType;
            Transaction = transaction;
        }
        public Guid MessageId { get; private set; }
        public User Sender { get; private set; }
        public User? Receiver { get; private set; }
        public Roles ReceiverRole { get; private set; }
        public string Text { get; private set; } = string.Empty;
        public string Subject { get; private set; } = string.Empty;
        public Message? BasedOnMessage { get; private set; }
        public MessageStatuses Status { get; private set; }
        public MessageType Type { get; private set; }
        public Transaction? Transaction { get; private set; }
    }

    public enum MessageStatuses
    {
        Sent,
        Delivered,
        Accepted,
        Replied
    }

    public enum MessageType
    {
        Inquery,
        CapitalReplenishment
    }
}
