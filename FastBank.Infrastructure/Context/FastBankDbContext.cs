using FastBank.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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

            modelBuilder.Entity<FriendsRelationDTO>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .IsRequired();

            modelBuilder.Entity<FriendsRelationDTO>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .IsRequired();

            var testCustomer = new UserDTO(Guid.NewGuid(), "Ivan", "1@1.com", DateTime.Now, "123", Roles.Customer, false);
            var customerFriend = new UserDTO(Guid.NewGuid(), "Ivan Friend", "2@2.com", DateTime.Now, "123", Roles.Customer, false); ;

            modelBuilder.Entity<UserDTO>().HasData(testCustomer);
            modelBuilder.Entity<UserDTO>().HasData(customerFriend);
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Ангел Ангелов", "achoceo@abv.bg", DateTime.Now, "achkata", Roles.Manager, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Анелия Иванова", "ani90@abv.bg", DateTime.Now, "anito1990", Roles.Accountant, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Добромир Иванов", "dobaIv@abv.bg", DateTime.Now, "dobbanker", Roles.Banker, false));
            modelBuilder.Entity<UserDTO>().HasData(new UserDTO(Guid.NewGuid(), "Камелия Ангелова", "kamiang@abv.bg", DateTime.Now, "kameliq1988", Roles.CustomerService, false));

            modelBuilder.Entity<BankDTO>().HasData(new BankDTO(10000m));

            modelBuilder.Entity<FriendsRelationDTO>().HasData(new FriendsRelationDTO(Guid.NewGuid(), testCustomer.ToDomainObj(), customerFriend.ToDomainObj(), false));
        }

        public virtual DbSet<UserDTO> Users { get; set; }
        public virtual DbSet<BankDTO> Banks { get; set; }
        public virtual DbSet<BankAccountDTO> BankAccounts { get; set; }
        public virtual DbSet<MessageDTO> Messages { get; set; }
        public virtual DbSet<FriendsRelationDTO> FriendsRelations { get; set;}

    }
}
