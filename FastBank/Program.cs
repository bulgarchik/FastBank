
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
            var serviceProvider = ConfigureServices();

            var repo = serviceProvider.GetService<IRepository>();

            Customer customer = new Customer { Name = "Bai Ivan" };

            repo.Add<Customer>(customer);

            MenuOptions.ShowMainMenu();

            var custumers = repo.Set<Customer>();

            foreach (var item in custumers)
            {
                Console.WriteLine(item.Name);
            }
            
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register the repository and DbContext
            services.AddScoped<IRepository, Repository>();
            services.AddDbContext<FastBankDbContext>();

            // Build the service provider
            return services.BuildServiceProvider();
        }
    }
}