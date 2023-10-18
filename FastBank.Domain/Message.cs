using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Domain
{
    public class Message
    {
        public Message(Guid messageId, Customer sender, Customer? receiver, Roles receiverRole, string text, string subject, Message? basedOnMessage)
        {
            MessageId = messageId;
            Sender = sender;
            Receiver = receiver;
            ReceiverRole = receiverRole;
            Text = text;
            Subject = subject;
            BasedOnMessage = basedOnMessage;
        }

        public Guid MessageId { get; private set; }
        public Customer Sender { get; private set; }
        public Customer? Receiver { get; private set; }
        public Roles ReceiverRole { get; private set; }
        public string Text { get; private set; } = string.Empty;
        public string Subject { get; private set; } = string.Empty;
        public Message? BasedOnMessage { get; private set; }

    }
}
