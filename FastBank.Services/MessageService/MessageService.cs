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

        public void GetMessages(Customer customer)
        {
            var messages = _messageRepo.GetCustomerMessages(customer);

            foreach (var message in messages)
            {
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"Text: {message.Text}");
                Console.WriteLine($"Status: {message.Status}");
                Console.WriteLine(new string('*', Console.WindowWidth));
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
            Console.ReadKey();
        }

        public void AddMessage(string subject,
                               string text,
                               MessageStatuses status,
                               MessageType type,
                               Customer sender,
                               Customer? receiver,
                               Roles receiverRole,
                               Message basedOnMessage)
        {
            Message message = new Message(Guid.NewGuid(), sender, receiver, receiverRole, text, subject, null, status, type);
            _messageRepo.Add(message);
        }

        public Message InputMessage(Customer customer)
        {
            Console.WriteLine("Please input message subject:");
            var subject = Console.ReadLine();

            Console.WriteLine("Please text message");
            var text = Console.ReadLine();

            var message = new Message(Guid.NewGuid(), customer, null, Roles.CustomerService,
                                      text, subject, null, MessageStatuses.Sent, MessageType.Inquery);
            
            AddMessage(message);
            return message;
        }
    }
}
