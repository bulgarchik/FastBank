using FastBank.Services;

namespace FastBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //MenuOptions.ShowMainMenu();

            ICustomerService customerService = new CustomerService();



           foreach (var item in customerService.GetAll())
            {
               Console.WriteLine(item.Name);
            }
        }
    }
}