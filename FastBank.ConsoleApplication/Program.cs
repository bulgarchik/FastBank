using FastBank.Domain;
using FastBank.Services;
using System.Text;

namespace FastBank
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            MenuOptions.ShowMainMenu();
        }
    }
}