using System.Text.RegularExpressions;

namespace FastBank.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUserService _userService;

        public MenuService() 
        {
            _userService = new UserService();
        }

        public void MoveToPreviousLine(ConsoleKeyInfo inputkey, int countOfLines = 1)
        {
            if (inputkey.Key != ConsoleKey.Enter)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
            }
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
            while (!regPattern.IsMatch(inputCommand ?? string.Empty))
            {
                Console.WriteLine("\nERROR: Please input correct command from menu. (press any key to continue..)");
                var inputKey = Console.ReadKey();
                MoveToPreviousLine(inputKey, menuOptions.Split("\n").Length + 3);
                Console.WriteLine(menuOptions);
                inputCommand = Console.ReadLine();

            }
            return Convert.ToInt32(inputCommand);
        }

        public void Logo()
        {
            Console.WriteLine();
            Console.WriteLine("-----------------------");
            Console.WriteLine("| FFFFFFFFF BBBBBBBB  |");
            Console.WriteLine("| FFFFFFFFF BBBBBBBBB |");
            Console.WriteLine("| FFF       BBB   BBB |");
            Console.WriteLine("| FFF       BBB   BBB |");
            Console.WriteLine("| FFFFFFFFF BBBBBBBBB |");
            Console.WriteLine("| FFFFFFFFF BBBBBBBBB |");
            Console.WriteLine("| FFF       BBB   BBB |");
            Console.WriteLine("| FFF       BBB   BBB |");
            Console.WriteLine("| FFF       BBBBBBBBB |");
            Console.WriteLine("| FFF       BBBBBBBB  |");
            Console.WriteLine("-----------------------");
            Console.WriteLine();
        }

        public string InputEmail()
        {
            Console.WriteLine("Please input you email:");
            Console.Write("Email: ");
            var email = Console.ReadLine();
            while (_userService.ValidateEmail(email ?? string.Empty, new List<string>()).Count > 0)
            {
                Console.WriteLine("You've inputted wrong email. Press any key to try again or type \"quit\" for exit");
                var keyIsEnter = Console.ReadKey();
                new MenuService().MoveToPreviousLine(keyIsEnter, 2);
                Console.Write("Email: ");
                email = Console.ReadLine()??string.Empty;
                if (email == "quit")
                return email;
            }
            return email ?? string.Empty;

        }
    }
}
