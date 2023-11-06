namespace FastBank.Domain.RepositoryInterfaces
{
    public interface IMessageRepository
    {
        public void Add(Message message);

        public void UpdateStatus(Message message, MessageStatuses newMsgStatus);

        public List<Message?> GetCustomerMessages(User user);

        public List<Message?> GetCustomerServiceMessages(User user);
    }
}
