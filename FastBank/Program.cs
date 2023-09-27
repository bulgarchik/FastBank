
using AutoMapper;
using FastBank.Infrastructure;
using Infrastructure.Context;

namespace FastBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FastBankDbContext db = new FastBankDbContext();
            var repo = new Repository(db);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, Infrastructure.Model.Customer>();
                cfg.CreateMap<Infrastructure.Model.Customer, Customer>().ReverseMap();
            });

            var mapper = config.CreateMapper();

            Customer customer = new Customer { Name = "Bai Ivan" };

            var customerModel = mapper.Map<Infrastructure.Model.Customer>(customer);

            repo.Add<Infrastructure.Model.Customer>(customerModel);

            MenuOptions.ShowMainMenu();

            var repoCustomers = repo.Set<Infrastructure.Model.Customer>();

            foreach (var repoItem in repoCustomers)
            {
                var item = mapper.Map<Customer>(repoItem);
                Console.WriteLine(item.Name);
            }
        }
    }
}