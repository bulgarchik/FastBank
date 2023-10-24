using System.Text.RegularExpressions;

namespace FastBank.Services
{
    public class MenuService : IMenuService
    {
        public void MoveToPreviousLine(int countOfLines = 1)
        {
            while (countOfLines > 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                countOfLines--;
            }

            Console.SetCursorPosition(0, Console.CursorTop);
        }

        public string PasswordStaredInput()
        {
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            return pass;
        }

        //TODO Move to MenuService
        public int CommandRead(Regex regPattern, string menuOptions)
        {
            Console.WriteLine(menuOptions);
            string? inputCommand = Console.ReadLine();
            while (!regPattern.IsMatch(inputCommand ?? ""))
            {
                Console.WriteLine("\nERROR: Please input correct command from menu. (press any key to continue..)");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine(menuOptions);
                inputCommand = Console.ReadLine();

            }
            return Convert.ToInt32(inputCommand);
        }
    }
}
