namespace FastBank.Domain.RepositoryInterfaces
{
    public interface IMessageRepository
    {
        public void Add(Message message);

        public void UpdateStatus(Message message, MessageStatus newMsgStatus);

        public List<Message?> GetCustomerMessages(User user);

        public List<Message?> GetCustomerServiceMessages(User user);

        public List<Message?> GetManagerMessages(User user);
    }
}
