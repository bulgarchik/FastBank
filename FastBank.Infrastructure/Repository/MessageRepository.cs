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

        public List<Message> GetCustomerMessages(Customer customer)
        {
            return _repo.SetNoTracking<MessageDTO>()
                    .Where(m => m.SenderId == customer.Id || m.ReceiverId == customer.Id)
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .Include(m => m.BasedOnMessage)
                    .Select(m => m.ToDomainObj())
                    .ToList();
        }
    }
}
