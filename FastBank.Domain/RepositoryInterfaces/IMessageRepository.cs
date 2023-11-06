﻿namespace FastBank.Domain.RepositoryInterfaces
{
    public interface IMessageRepository
    {
        public void Add(Message message);

        public List<Message?> GetCustomerMessages(User user);

        public List<Message?> GetCustomerServiceMessages(User user);
    }
}
