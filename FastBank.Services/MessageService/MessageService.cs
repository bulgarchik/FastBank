using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using FastBank.Infrastructure.Repository;

namespace FastBank.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepo;

        public MessageService()
        {
            _messageRepo = new MessageRepository();
        }
        public void AddMessage(Message message)
        {
            _messageRepo.Add(message);
        }

        public void AddMessage(string subject, string text, MessageStatuses status, MessageType type, Customer sender, Customer? receiver, Roles receiverRole, Message basedOnMessage)
        {
            Message message = new Message(Guid.NewGuid(), sender, receiver, receiverRole, text, subject, null, status, type);
            _messageRepo.Add(message);
        }
    }
}
