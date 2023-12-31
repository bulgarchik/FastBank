﻿using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;

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

        public void UpdateStatus(Message message, MessageStatus newMsgStatus)
        {
            message.UpdateMessageStatus(newMsgStatus);

            var msgDto = _repo.Set<MessageDTO>().Where(m => m.MessageId == message.MessageId).FirstOrDefault();
            if (msgDto != null)
            {
                msgDto.UpdateMessageStatus(message.MessageStatus);
                _repo.Update<MessageDTO>(msgDto);
            }
        }

        public List<Message?> GetCustomerMessages(User user)
        {
            var dbMessages = _repo.SetNoTracking<MessageDTO>()
                    .Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id)
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .Include(m => m.BasedOnMessage)
                    .OrderBy(m => m.CreatedOn)
                    .ToList();
            var messages = dbMessages.Select(m => m.ToDomainObj()).ToList();
            
            return messages;
        }

        public List<Message?> GetCustomerServiceMessages(User user)
        {
            var dbMessages = _repo.SetNoTracking<MessageDTO>()
                    .Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id || m.ReceiverRole == Role.CustomerService)
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .Include(m => m.BasedOnMessage)
                    .Include(m => m.TransactionOrder.FromBankAccount.User)
                    .Include(m => m.TransactionOrder.ToBankAccount.User)
                    .Include(m => m.TransactionOrder.Bank)
                    .Include(m => m.TransactionOrder.OrderedByUser)
                    .OrderBy(m => m.CreatedOn)
                    .ToList();

            var messages = dbMessages.Select(m => m.ToDomainObj()).ToList();
            return messages;
        }

        public List<Message?> GetManagerMessages(User user)
        {
            var dbMessages = _repo.SetNoTracking<MessageDTO>()
                    .Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id || m.ReceiverRole == Role.Manager)
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .Include(m => m.BasedOnMessage)
                    .Include(m => m.Transaction.BankAccount.User)
                    .Include(m => m.Transaction.Bank)
                    .Include(m => m.Transaction.CreatedByUser)
                    .OrderBy(m => m.CreatedOn)
                    .ToList();

            var messages = dbMessages.Select(m => m.ToDomainObj()).ToList();
            return messages;
        }
    }
}
