
using FastBank.Infrastructure;
using Infrastructure.Context;
using Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace FastBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FastBankDbContext db = new FastBankDbContext();
            var repo = new Repository(db);

            Customer customer = new Customer { Name = "Bai Ivan" };

            repo.Add<Customer>(customer);

            MenuOptions.ShowMainMenu();

            var customers = repo.Set<Customer>();

            foreach (var item in customers)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}