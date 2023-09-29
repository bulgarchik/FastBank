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
        }

        public virtual DbSet<CustomerDTO> Customers { get; set; }
    }
}
