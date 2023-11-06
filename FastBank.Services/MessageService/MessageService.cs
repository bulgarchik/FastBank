using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using FastBank.Infrastructure.Repository;

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

        public List<Message?> GetMessages(User user)
        {
            var messages = new List<Message?>();

            if (user.Role == Roles.CustomerService)
            {
                messages = _messageRepo.GetCustomerServiceMessages(user);
            }
            else if (user.Role == Roles.Customer)
            {
                messages = _messageRepo.GetCustomerMessages(user);
            }

            return messages;
        }

        public void AddMessage(
            string subject,
            string text,
            MessageStatuses status,
            MessageType type,
            User sender,
            User? receiver,
            Roles receiverRole,
            Message? basedOnMessage,
            Transaction? transaction)
        {
            Message message = new Message(Guid.NewGuid(), sender, receiver, receiverRole, text, subject, basedOnMessage, status, type, transaction);
            _messageRepo.Add(message);
        }

        public Message InputMessage(User user)
        {
            Console.WriteLine("Please input message subject:");
            Console.Write("Subject: ");
            var subject = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Please text message");
            Console.Write("Text: ");
            var text = Console.ReadLine() ?? string.Empty;

            var message = new Message(Guid.NewGuid(), user, null, Roles.CustomerService,
                                      text, subject, null, MessageStatuses.Sent, MessageType.Inquery, null);

            _messageRepo.Add(message);
            return message;
        }

        public void ShowMessagesMenu(User user, List<Message?>? messages = null)
        {
            Console.Clear();
            _menuService.ShowLogo();
            if (messages == null)
            {
                messages = GetMessages(user);
            }
            ShowMessages(messages);

            var menuOptions = $"\nPlease choose your action: " +
                              $"\n  0: for exit";
            int action = _menuService.CommandRead(1, menuOptions);

            switch (action)
            {
                case 0: return;
            }
        }

        public void ShowMessages(List<Message?> messages)
        {
            if (messages.Count > 0)
            {
                Console.WriteLine($"Messages list:");
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
            
            foreach (var message in messages)
            {
                Console.WriteLine(new string('*', Console.WindowWidth));
                Console.WriteLine($"Message number: {message?.Index}");
                Console.WriteLine($"\nSubject: {message?.Subject}");
                Console.WriteLine($"\nText: {message?.Text}");
                Console.WriteLine($"\nStatus: {message?.Status}");
                Console.WriteLine(new string('*', Console.WindowWidth));
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
        }
    }
}
