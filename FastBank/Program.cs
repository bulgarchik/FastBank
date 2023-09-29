using FastBank.Services;

namespace FastBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MenuOptions.ShowMainMenu();

            ICustomerService customerService = new CustomerService();

            var customer = new Customer(Guid.NewGuid(),"Alex", "a@a.com",DateTime.Now,"123", Roles.Manager);

            customerService.Add(customer);

           foreach (var item in customerService.GetAll())
            {
               Console.WriteLine(item.Name);
            }
        }
    }
}