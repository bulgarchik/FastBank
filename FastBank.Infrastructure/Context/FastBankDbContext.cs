﻿using FastBank.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastBank.Infrastructure.Context
{
    public class FastBankDbContext : DbContext
    {
        public FastBankDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "MyInMemoryDatabase");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccountDTO>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .IsRequired();

            modelBuilder.Entity<MessageDTO>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .IsRequired();

            modelBuilder.Entity<MessageDTO>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId);

            modelBuilder.Entity<MessageDTO>()
                .HasOne(m => m.BasedOnMessage)
                .WithMany()
                .HasForeignKey(m => m.BasedOnMessageId);

            modelBuilder.Entity<MessageDTO>()
                .HasOne(m => m.Transaction)
                .WithMany()
                .HasForeignKey(m => m.TransactionId);

            modelBuilder.Entity<MessageDTO>()
                .HasOne(m => m.TransactionOrder)
                .WithMany()
                .HasForeignKey(m => m.TransactionOrderId);

            modelBuilder.Entity<UserFriendDTO>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .IsRequired();

            modelBuilder.Entity<UserFriendDTO>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .IsRequired();

            modelBuilder.Entity<TransactionDTO>()
                .HasOne(t => t.CreatedByUser)
                .WithMany()
                .HasForeignKey(t => t.CreatedByUserId)
                .IsRequired();
            modelBuilder.Entity<TransactionDTO>()
                .HasOne(t => t.Bank)
                .WithMany()
                .HasForeignKey(t => t.BankId);
            modelBuilder.Entity<TransactionDTO>()
                .HasOne(t => t.BankAccount)
                .WithMany()
                .HasForeignKey(t => t.BankAccountId);

            modelBuilder.Entity<TransactionOrderDTO>()
                .HasOne(to => to.Bank)
                .WithMany().
                HasForeignKey(to => to.BankId);
            modelBuilder.Entity<TransactionOrderDTO>()
                .HasOne(to => to.ToBankAccount)
                .WithMany()
                .HasForeignKey(to => to.ToBankAccountId);
            modelBuilder.Entity<TransactionOrderDTO>()
                .HasOne(to => to.FromBankAccount)
                .WithMany()
                .HasForeignKey(to => to.FromBankAccountId);
            modelBuilder.Entity<TransactionOrderDTO>()
                .HasOne(to => to.OrderedByUser)
                .WithMany()
                .HasForeignKey(to => to.OrderedByUserId)
                .IsRequired();

            modelBuilder.Entity<TransactionsFileReportDTO>()
                .HasOne(to => to.CreatedBy).WithMany().HasForeignKey(to => to.UserId).IsRequired();    

            modelBuilder.Entity<EmployeeDTO>()
                .HasOne(em => em.User)
                .WithMany()
                .HasForeignKey(em => em.UserId);

            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Ангел Ангелов", "achoceo@abv.bg", DateTime.Now, "achkata", Role.Manager, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Анелия Иванова", "ani90@abv.bg", DateTime.Now, "anito1990", Role.Accountant, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Добромир Иванов", "dobaIv@abv.bg", DateTime.Now, "dobbanker", Role.Banker, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Камелия Ангелова", "kamiang@abv.bg", DateTime.Now, "kameliq1988", Role.CustomerService, false));

            modelBuilder.Entity<BankDTO>().HasData(new BankDTO(10000m));

            //seed data


            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Ангел Ангелов", "m@m.mm", DateTime.Now, "m", Role.Manager, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Стоян Косев", "s@s.ss", DateTime.Now, "s", Role.Accountant, false));


            var testCustomer = new UserDTO(Guid.NewGuid(), "Александър Георгиев", "1@1.com", DateTime.Now, "1", Role.Customer, false);
            var customerFriend = new UserDTO(Guid.NewGuid(), "Ангел Костадинов", "2@2.com", DateTime.Now, "1", Role.Customer, false);
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Васили Иванов", "3@3.bg", DateTime.Now, "1", Role.CustomerService, false));

            var bankAccountCustomer = new Domain.BankAccount(Guid.NewGuid(), testCustomer.ToDomainObj(), 100);

            modelBuilder.Entity<BankAccountDTO>().HasData(new BankAccountDTO(bankAccountCustomer));

            modelBuilder.Entity<TransactionDTO>().HasData(new TransactionDTO(new Domain.Transaction(testCustomer.ToDomainObj(), 100, null, bankAccountCustomer, TransactionType.BankAccountTransaction)));
            modelBuilder.Entity<TransactionDTO>().HasData(new TransactionDTO(new Domain.Transaction(testCustomer.ToDomainObj(), 20, null, bankAccountCustomer, TransactionType.BankAccountTransaction)));
            modelBuilder.Entity<TransactionDTO>().HasData(new TransactionDTO(new Domain.Transaction(testCustomer.ToDomainObj(), 10, null, bankAccountCustomer, TransactionType.BankAccountTransaction)));
            modelBuilder.Entity<TransactionDTO>().HasData(new TransactionDTO(new Domain.Transaction(testCustomer.ToDomainObj(), -10, null, bankAccountCustomer, TransactionType.BankAccountTransaction)));
            modelBuilder.Entity<TransactionDTO>().HasData(new TransactionDTO(new Domain.Transaction(testCustomer.ToDomainObj(), -10, null, bankAccountCustomer, TransactionType.BankAccountTransaction)));
            modelBuilder.Entity<TransactionDTO>().HasData(new TransactionDTO(new Domain.Transaction(testCustomer.ToDomainObj(), -20, null, bankAccountCustomer, TransactionType.BankAccountTransaction)));
            modelBuilder.Entity<TransactionDTO>().HasData(new TransactionDTO(new Domain.Transaction(testCustomer.ToDomainObj(), 30, null, bankAccountCustomer, TransactionType.BankAccountTransaction)));
            modelBuilder.Entity<TransactionDTO>().HasData(new TransactionDTO(new Domain.Transaction(testCustomer.ToDomainObj(), 10, null, bankAccountCustomer, TransactionType.BankAccountTransaction)));
            modelBuilder.Entity<TransactionDTO>().HasData(new TransactionDTO(new Domain.Transaction(testCustomer.ToDomainObj(), 10, null, bankAccountCustomer, TransactionType.BankAccountTransaction)));


            modelBuilder.Entity<UserDTO>().HasData(testCustomer);
            modelBuilder.Entity<UserDTO>().HasData(customerFriend);
            modelBuilder.Entity<UserFriendDTO>().HasData(new UserFriendDTO(Guid.NewGuid(), testCustomer.ToDomainObj(), customerFriend.ToDomainObj(), false));
            var msg1 = new MessageDTO(
                    new Domain.Message(
                            Guid.NewGuid(),
                            DateTime.UtcNow.AddHours(-6),
                            testCustomer.ToDomainObj(),
                            null,
                            Role.CustomerService,
                            "First message text to Customer Service",
                            "First Message",
                            null,
                            Domain.MessageStatus.Sent,
                            Domain.MessageType.Inquiry, null)
                    );

            var msg2 = new MessageDTO(
                    new Domain.Message(
                            Guid.NewGuid(),
                            DateTime.UtcNow.AddHours(-5),
                            testCustomer.ToDomainObj(),
                            null,
                            Role.CustomerService,
                            "Second message text to Customer Service",
                            "Second Message",
                            null,
                            Domain.MessageStatus.Sent,
                            Domain.MessageType.Inquiry, null)
                    );

            var msg3 = new MessageDTO(
                   new Domain.Message(
                           Guid.NewGuid(),
                           DateTime.UtcNow.AddHours(-4),
                           testCustomer.ToDomainObj(),
                           null,
                           Role.CustomerService,
                           "Reply to first message",
                           "Reply to first Message",
                           msg1.ToDomainObj(),
                           Domain.MessageStatus.Sent,
                           Domain.MessageType.Inquiry, null));

            var msg4 = new MessageDTO(
                 new Domain.Message(
                         Guid.NewGuid(),
                         DateTime.UtcNow.AddHours(-3),
                         testCustomer.ToDomainObj(),
                         null,
                         Role.CustomerService,
                         "Another one message text to Customer Service",
                         "Another message",
                         null,
                         Domain.MessageStatus.Sent,
                         Domain.MessageType.Inquiry, null));

            var msg5 = new MessageDTO(
                 new Domain.Message(
                         Guid.NewGuid(),
                         DateTime.UtcNow.AddHours(-2),
                         testCustomer.ToDomainObj(),
                         null,
                         Role.CustomerService,
                         "Question to Customer Service",
                         "Message",
                         msg3.ToDomainObj(),
                         Domain.MessageStatus.Sent,
                         Domain.MessageType.Inquiry, null));

            var msg6 = new MessageDTO(
                 new Domain.Message(
                         Guid.NewGuid(),
                         DateTime.UtcNow.AddHours(-1),
                         testCustomer.ToDomainObj(),
                         null,
                         Role.CustomerService,
                         "Problem with account, please fix the problem",
                         "Problem with account",
                         msg4.ToDomainObj(),
                         Domain.MessageStatus.Sent,
                         Domain.MessageType.Inquiry, null));

            modelBuilder.Entity<MessageDTO>().HasData(msg1);
            modelBuilder.Entity<MessageDTO>().HasData(msg2);
            modelBuilder.Entity<MessageDTO>().HasData(msg3);
            modelBuilder.Entity<MessageDTO>().HasData(msg4);
            modelBuilder.Entity<MessageDTO>().HasData(msg5);
            modelBuilder.Entity<MessageDTO>().HasData(msg6);
        }

        public virtual DbSet<UserDTO> Users { get; set; }
        public virtual DbSet<BankDTO> Banks { get; set; }
        public virtual DbSet<BankAccountDTO> BankAccounts { get; set; }
        public virtual DbSet<MessageDTO> Messages { get; set; }
        public virtual DbSet<UserFriendDTO> FriendsRelations { get; set; }
        public virtual DbSet<TransactionDTO> Transactions { get; set; }
        public virtual DbSet<TransactionOrderDTO> TransactionsOrder { get; set; }
        public virtual DbSet<TransactionsFileReportDTO> TransactionsReports { get; set; }
        public virtual DbSet<EmployeeDTO> Employees { get; set; }
    }
}
