using FastBank.Domain;

namespace FastBank.Services
{
    public interface IMessageService
    {
        public List<Message?> GetMessages(User user);

        public void AddMessage(
            string subject,
            string text,
            MessageStatus status,
            MessageType type,
            User sender,
            User? receiver,
            Role receiverRole,
            Message? basedOnMessage,
            Transaction? transaction = null,
            TransactionOrder? transactionOrder = null);

        public Message InputMessage(User user);

        public Message ReplyToMessage(User user, Message message);

        public Message? SelectMessageByInputId(List<Message?>? messages);

        public void ShowMessagesMenu(User user, List<Message?>? messages = null);

        public void ShowMessageMenu(User user, Message message, List<Message> messages);

        public void ShowMessages(List<Message?> messages, User user, bool heirarchy);

        public void ShowMessageDetails(User user, Message message, List<Message> messages);
    }
}
