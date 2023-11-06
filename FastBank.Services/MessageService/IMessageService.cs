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
        public List<Message?> GetMessages(User user);

        public void AddMessage(
            string Subject,
            string text,
            MessageStatuses status,
            MessageType type,
            User sender,
            User? receiver,
            Roles receiverRole,
            Message? basedOnMessage,
            Transaction transaction);

        public Message InputMessage(User user);

        public Message ReplayToMessage(User user, Message message);

        public Message? SelectMessageByInputId(List<Message?>? messages);

        public void ShowMessagesMenu(User user, List<Message?>? messages = null);

        public void ShowMessageMenu(User user, Message message);

        public void ShowMessages(List<Message?> messages);

        public void ShowMessageDetails(User user, Message message);
    }
}
