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
                .HasOne(c => c.Customer)
                .WithMany(c => c.BankAccounts)
                .HasForeignKey(c => c.CustomerId)
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
                .WithMany().
                HasForeignKey(m => m.BasedOnMessageId);

            modelBuilder.Entity<CustomerDTO>().HasData(new CustomerDTO(new FastBank.Customer(Guid.NewGuid(), "Ivan", "1@1.com", DateTime.Now, "123", Roles.Customer, false)));
            modelBuilder.Entity<CustomerDTO>().HasData(new CustomerDTO(new FastBank.Customer(Guid.NewGuid(), "Ангел Ангелов", "achoceo@abv.bg", DateTime.Now, "achkata", Roles.Manager, false)));
            modelBuilder.Entity<CustomerDTO>().HasData(new CustomerDTO(new FastBank.Customer(Guid.NewGuid(), "Анелия Иванова", "ani90@abv.bg", DateTime.Now, "anito1990", Roles.Accountant, false)));
            modelBuilder.Entity<CustomerDTO>().HasData(new CustomerDTO(new FastBank.Customer(Guid.NewGuid(), "Добромир Иванов", "dobaIv@abv.bg", DateTime.Now, "dobbanker", Roles.Banker, false)));
            modelBuilder.Entity<CustomerDTO>().HasData(new CustomerDTO(new FastBank.Customer(Guid.NewGuid(), "Камелия Ангелова", "kamiang@abv.bg", DateTime.Now, "kameliq1988", Roles.CustomerService, false)));

            modelBuilder.Entity<BankDTO>().HasData(new BankDTO(10000m));
        }

        public virtual DbSet<CustomerDTO> Customers { get; set; }
        public virtual DbSet<BankDTO> Banks { get; set; }
        public virtual DbSet<BankAccountDTO> BankAccounts { get; set; }
        public virtual DbSet<MessageDTO> Messages { get; set; }

    }
}
