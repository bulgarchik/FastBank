using FastBank.Services;

namespace FastBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //MenuOptions.ShowMainMenu();

            ICustomerService cs = new CustomerService();

           foreach (var item in cs.GetAll())
            {
               Console.WriteLine(item.Name);
            }
        }
    }
}