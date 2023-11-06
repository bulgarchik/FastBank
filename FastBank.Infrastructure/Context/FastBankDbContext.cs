using FastBank.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Data;

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
                       
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Ангел Ангелов", "achoceo@abv.bg", DateTime.Now, "achkata", Role.Manager, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Анелия Иванова", "ani90@abv.bg", DateTime.Now, "anito1990", Role.Accountant, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Добромир Иванов", "dobaIv@abv.bg", DateTime.Now, "dobbanker", Role.Banker, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Камелия Ангелова", "kamiang@abv.bg", DateTime.Now, "kameliq1988", Role.CustomerService, false));

            modelBuilder.Entity<BankDTO>().HasData(new BankDTO(10000m));
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
