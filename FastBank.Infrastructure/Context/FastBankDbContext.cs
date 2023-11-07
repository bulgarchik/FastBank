using FastBank.Infrastructure.DTOs;
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

            modelBuilder.Entity<TransactionOrderDto>()
                .HasOne(to => to.Bank)
                .WithMany().
                HasForeignKey(to => to.BankId);
            modelBuilder.Entity<TransactionOrderDto>()
                .HasOne(to => to.ToBankAccount)
                .WithMany()
                .HasForeignKey(to => to.ToBankAccountId);
            modelBuilder.Entity<TransactionOrderDto>()
                .HasOne(to => to.FromBankAccount)
                .WithMany()
                .HasForeignKey(to => to.FromBankAccountId);
            modelBuilder.Entity<TransactionOrderDto>()
                .HasOne(to => to.OrderedByUser)
                .WithMany()
                .HasForeignKey(to => to.OrderedByUserId)
                .IsRequired();
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Ангел Ангелов", "achoceo@abv.bg", DateTime.Now, "achkata", Role.Manager, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Анелия Иванова", "ani90@abv.bg", DateTime.Now, "anito1990", Role.Accountant, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Добромир Иванов", "dobaIv@abv.bg", DateTime.Now, "dobbanker", Role.Banker, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Камелия Ангелова", "kamiang@abv.bg", DateTime.Now, "kameliq1988", Role.CustomerService, false));

            modelBuilder.Entity<BankDTO>().HasData(new BankDTO(10000m));

            var testCustomer = new UserDTO(Guid.NewGuid(), "Ivan", "1@1.com", DateTime.Now, "123", Role.Customer, false);
            var customerFriend = new UserDTO(Guid.NewGuid(), "Ivan Friend", "2@2.com", DateTime.Now, "123", Role.Customer, false);
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Костюмер Сервисович", "2@2.bg", DateTime.Now, "1", Role.CustomerService, false));

            modelBuilder.Entity<UserDTO>().HasData(testCustomer);
            modelBuilder.Entity<UserDTO>().HasData(customerFriend);
            modelBuilder.Entity<UserFriendDTO>().HasData(new UserFriendDTO(Guid.NewGuid(), testCustomer.ToDomainObj(), customerFriend.ToDomainObj(), false));
            modelBuilder.Entity<MessageDTO>().HasData(
                new MessageDTO(
                    new Domain.Message(
                            Guid.NewGuid(),
                            testCustomer.ToDomainObj(),
            null,
                            Role.CustomerService,
                            "First Message Subjectr",
                            "First message text to Customer Service",
                            null,
                            Domain.MessageStatus.Sent,
                            Domain.MessageType.Inquery, null)
                    ));

            modelBuilder.Entity<MessageDTO>().HasData(
                new MessageDTO(
                    new Domain.Message(
                            Guid.NewGuid(),
                            testCustomer.ToDomainObj(),
                            null,
                            Role.CustomerService,
                            "Second Message Subjectr",
                            "Second message text to Customer Service",
                            null,
                            Domain.MessageStatus.Sent,
                            Domain.MessageType.Inquery, null)
                    ));

        }

        public virtual DbSet<UserDTO> Users { get; set; }
        public virtual DbSet<BankDTO> Banks { get; set; }
        public virtual DbSet<BankAccountDTO> BankAccounts { get; set; }
        public virtual DbSet<MessageDTO> Messages { get; set; }
        public virtual DbSet<UserFriendDTO> FriendsRelations { get; set;}
        public virtual DbSet<TransactionDTO> Transactions { get; set; }
        public virtual DbSet<TransactionOrderDto> TransactionsOrder { get; set; }
    }
}
