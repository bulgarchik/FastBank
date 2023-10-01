using FastBank.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
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
            modelBuilder.Entity<CustomerDTO>().HasData(new CustomerDTO(new FastBank.Customer(Guid.NewGuid(), "Ivan", "1@1.com", DateTime.Now, "123", FastBank.Roles.Customer)));
            modelBuilder.Entity<CustomerDTO>().HasData(new CustomerDTO(new FastBank.Customer(Guid.NewGuid(), "Ангел Ангелов", "achoceo@abv.bg", DateTime.Now, "achkata", FastBank.Roles.Manager)));
            modelBuilder.Entity<CustomerDTO>().HasData(new CustomerDTO(new FastBank.Customer(Guid.NewGuid(), "Анелия Иванова", "ani90@abv.bg", DateTime.Now, "anito1990", FastBank.Roles.Accunter)));
            modelBuilder.Entity<CustomerDTO>().HasData(new CustomerDTO(new FastBank.Customer(Guid.NewGuid(), "Добромир Иванов", "dobaIv@abv.bg", DateTime.Now, "dobbanker ", FastBank.Roles.Banker)));
            modelBuilder.Entity<CustomerDTO>().HasData(new CustomerDTO(new FastBank.Customer(Guid.NewGuid(), "Камелия Ангелова", "kamiang@abv.bg", DateTime.Now, "kameliq1988", FastBank.Roles.CustomerService)));

            modelBuilder.Entity<BankDTO>().HasData(new BankDTO(10000));
        }

        public virtual DbSet<CustomerDTO> Customers { get; set; }
        public virtual DbSet<BankDTO> Banks { get; set; }

    }
}
