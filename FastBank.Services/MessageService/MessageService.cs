using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using FastBank.Infrastructure.Repository;
using System.Text.RegularExpressions;

namespace FastBank.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IMenuService _menuService;

        public MessageService()
        {
            _messageRepo = new MessageRepository();
            _menuService = new MenuService();
        }

        public void AddMessage(Message message)
        {
            _messageRepo.Add(message);
        }

        public void GetMessages(User user)
        {
            var messages = _messageRepo.GetCustomerMessages(user);

            if (messages.Count > 0)
            {
                Console.WriteLine($"Messages list:");
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
            int mesNumber = 0;
            foreach (var message in messages)
            {
                mesNumber++;
                Console.WriteLine(new string('*', Console.WindowWidth));
                Console.WriteLine($"Message number: {mesNumber}");
                Console.WriteLine($"\nSubject: {message.Subject}");
                Console.WriteLine($"\nText: {message.Text}");
                Console.WriteLine($"\nStatus: {message.Status}");
                Console.WriteLine(new string('*', Console.WindowWidth));
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
        }

        public void AddMessage(
            string subject,
            string text,
            MessageStatuses status,
            MessageType type,
            User sender,
            User? receiver,
            Roles receiverRole,
            Message basedOnMessage)
        {
            Message message = new Message(Guid.NewGuid(), sender, receiver, receiverRole, text, subject, null, status, type);
            _messageRepo.Add(message);
        }

        public Message InputMessage(User user)
        {
            Console.WriteLine("Please input message subject:");
            Console.Write("Subject: ");
            var subject = Console.ReadLine();

            Console.WriteLine("Please text message");
            Console.Write("Text: ");
            var text = Console.ReadLine();

            var message = new Message(Guid.NewGuid(), user, null, Roles.CustomerService,
                                      text, subject, null, MessageStatuses.Sent, MessageType.Inquery);

            _messageRepo.Add(message);
            return message;
        }

        public void ShowMessagesMenu(User user)
        {
            Console.Clear();
            _menuService.Logo();
            GetMessages(user);
            var menuOptions = $"\nPlease choose your action: " +
                              $"\n  0: for exit";
            int action = _menuService.CommandRead(new Regex("^[0]{1}$"), menuOptions);
            
            switch (action)
            {
                case 0: return;
            }
        }
    }
}
