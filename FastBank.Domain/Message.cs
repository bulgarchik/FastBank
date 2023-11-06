namespace FastBank.Domain
{
    public class Message
    {
        public Message(
            Guid messageId,
            User sender,
            User? receiver,
            Role receiverRole,
            string text,
            string subject,
            Message? basedOnMessage,
            MessageStatus messageStatuses,
            MessageType messageType,
            Transaction? transaction,
            int index = 0)
        {
            MessageId = messageId;
            Sender = sender;
            Receiver = receiver;
            ReceiverRole = receiverRole;
            Text = text;
            Subject = subject;
            BasedOnMessage = basedOnMessage;
            MessageStatus = messageStatuses;
            MessageType = messageType;
            Transaction = transaction;
            Index = index;
        }
        public Guid MessageId { get; private set; }
        public User Sender { get; private set; }
        public User? Receiver { get; private set; }
        public Role ReceiverRole { get; private set; }
        public string Text { get; private set; } = string.Empty;
        public string Subject { get; private set; } = string.Empty;
        public Message? BasedOnMessage { get; private set; }
        public MessageStatus MessageStatus { get; private set; }
        public MessageType MessageType { get; private set; }
        public Transaction? Transaction { get; private set; }
        public int Index { get; private set; }

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
        Inquery,
        CapitalReplenishment
    }
}
