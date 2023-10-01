using FastBank.Services;

namespace FastBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ICustomerService customerService = new CustomerService();

            MenuOptions.ShowMainMenu();
        }
    }
}