using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.DTOs;

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
    }
}
