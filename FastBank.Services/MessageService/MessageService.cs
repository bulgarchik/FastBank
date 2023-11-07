using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using FastBank.Infrastructure.Repository;
using System.Data;

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

            if (user.Role == Role.CustomerService)
            {
                messages = _messageRepo.GetCustomerServiceMessages(user);
            }
            else if (user.Role == Role.Customer)
            {
                messages = _messageRepo.GetCustomerMessages(user);
            }

            return messages;
        }

        public void AddMessage(
            string subject,
            string text,
            MessageStatus status,
            MessageType type,
            User sender,
            User? receiver,
            Role receiverRole,
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

            var message = new Message(Guid.NewGuid(), user, null, Role.CustomerService,
                                      text, subject, null, MessageStatus.Sent, MessageType.Inquery, null);

            _messageRepo.Add(message);
            return message;
        }

        public Message ReplyToMessage(User user, Message message)
        {
            Console.WriteLine("Please intput replay to text message");
            Console.Write("Reply text: ");
            var text = Console.ReadLine() ?? string.Empty;

            var replayMessage = new Message(
                                        Guid.NewGuid(), 
                                        user, 
                                        message.Sender, 
                                        Role.Customer,
                                        text, 
                                        $"Re: {message.Subject}",
                                        message, 
                                        MessageStatus.Sent, 
                                        MessageType.Inquery, 
                                        null);

            _messageRepo.Add(replayMessage);
            _messageRepo.UpdateStatus(message, MessageStatus.Replied);

            return replayMessage;
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
                              $"\n1: Open message;  0: for exit";
            int action = _menuService.CommandRead(2, menuOptions);

            switch (action)
            {
                case 0: return;
                case 1:
                    {
                       if (messages != null)
                        {
                            var msg = SelectMessageByInputId(messages);
                            if (msg != null)
                            {
                                ShowMessageMenu(user, msg);
                            }
                        }
                        else
                        {
                            Console.WriteLine("You have no messages. Press any key to continue...");
                            var keyIsEnter = Console.ReadKey();
                            return;
                        }

                        break;
                    }
            }
        }

        public Message? SelectMessageByInputId(List<Message?>? messages)
        {
            if (messages == null)
            {
                return null;
            }

            int msgId;
            do
            {
                Console.WriteLine($"To open please input message ID from the list (type 'q' for exit):");
                Console.Write("Message ID:");
                var inputMsgId = Console.ReadLine();
                if (inputMsgId == "q")
                    return null;

                if (!int.TryParse(inputMsgId, out msgId) || msgId < 1 || msgId > messages.Count)
                {
                    Console.WriteLine("Please input correct message ID (press any key to continue...)");
                    var keyIsEnter = Console.ReadKey();
                    new MenuService().MoveToPreviousLine(keyIsEnter, 3);
                }

            }while (msgId < 1 || msgId > messages.Count);

            return messages.FirstOrDefault(m => m?.Index == msgId);
        }

        public void ShowMessageMenu(User user, Message message)
        {
            if (message == null)
            {
                return;
            }

            Console.Clear();
            _menuService.ShowLogo();

            ShowMessageDetails(user, message);

            var menuOptions = $"\nPlease choose your action: " +
                             $"\n1: Reply to message. 0: for exit.";
            int action = _menuService.CommandRead(2, menuOptions);

            switch (action)
            {
                case 0: return;
                case 1:
                    {
                        ReplyToMessage(user, message);
                        break;
                    }
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
                Console.WriteLine($"Message ID: {message?.Index}; " +
                                  $"Status: {message?.MessageStatus}; " +
                                  $"Subject: {message?.Subject}; " +
                                  $"{(message?.BasedOnMessage!=null ? $"Based on message ID:{message?.BasedOnMessage.Index}" : string.Empty)}");
                Console.WriteLine($"Text: {message?.Text}");
                Console.WriteLine(new string('*', Console.WindowWidth));
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
        }

        public void ShowMessageDetails(User user, Message message)
        {
            if (message.MessageStatus == MessageStatus.Sent && message.Sender != null && message.Sender != user)
            {
                _messageRepo.UpdateStatus(message, MessageStatus.Delivered);
            }

            Console.WriteLine(new string('*', Console.WindowWidth));
            Console.WriteLine($"Status: {message?.MessageStatus};\nSubject: {message?.Subject};");
            Console.WriteLine($"Text: {message?.Text}");
            Console.WriteLine(new string('*', Console.WindowWidth));
            Console.WriteLine(new string(' ', Console.WindowWidth));
        }
    }
}
