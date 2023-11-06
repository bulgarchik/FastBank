using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastBank.Infrastructure.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly Repository _repo;

        public MessageRepository()
        {
            _repo = new Repository(new FastBankDbContext());
        }

        public void Add(Message message)
        {
            _repo.Add<MessageDTO>(new MessageDTO(message));
        }

        public List<Message?> GetCustomerMessages(User user)
        {
            var messagesDTO = _repo.SetNoTracking<MessageDTO>()
                    .Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id)
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .Include(m => m.BasedOnMessage)
                    .ToList();
            var messages = messagesDTO.Select((m, c) => m.ToDomainObj(c + 1)).ToList();
            return messages;
        }   
        public List<Message?> GetCustomerServiceMessages(User user)
        {
            var messagesDTO = _repo.SetNoTracking<MessageDTO>()
                    .Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id || m.ReceiverRole == Roles.CustomerService)
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .Include(m => m.BasedOnMessage)
                    .ToList();

            var messages = messagesDTO.Select((m, c) => m.ToDomainObj(c + 1)).ToList();
            return messages;
        }
    }
}
