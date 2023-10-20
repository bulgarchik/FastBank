using FastBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Services.MessageService
{
    public interface IMessageService
    {
        public void AddMessage(string Subject,string text, MessageStatuses status, MessageType type, Customer sender, Customer? receiver, Roles receiverRole, Message basedOnMessage)
    }
}
