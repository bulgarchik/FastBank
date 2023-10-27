﻿using FastBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Services.MessageService
{
    public interface IMessageService
    {
        public void GetMessages(User user);

        public void AddMessage(
            string Subject,
            string text,
            MessageStatuses status,
            MessageType type,
            User sender,
            User? receiver,
            Roles receiverRole,
            Message basedOnMessage);

        public Message InputMessage(User user);
    }
}
