using System.Text;
using System.Text.RegularExpressions;

namespace FastBank.Services
{
    public class MenuService : IMenuService
    {
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

        public int CommandRead(int countOfOptions, string menuOptions, int startFromIndex = 0)
        {
            var sb = new StringBuilder();

            for (int i = startFromIndex; i < countOfOptions; i++)
            {
                sb.Append(i);
            }
            var regPattern = new Regex($"^[{sb?.ToString()}]{{1}}$");
            Console.WriteLine(menuOptions);
            Console.Write("\nAction: ");
            string? inputCommand = Console.ReadLine();
            while (!regPattern.IsMatch(inputCommand ?? string.Empty))
            {
                Console.WriteLine("\nERROR: Please input correct command from menu. (press any key to continue..)");
                var inputKey = Console.ReadKey();
                MoveToPreviousLine(inputKey, menuOptions.Split("\n").Length + 4);
                Console.WriteLine(menuOptions);
                Console.Write("\nAction: ");
                inputCommand = Console.ReadLine();
            }
            Console.WriteLine();
            return Convert.ToInt32(inputCommand);
        }

        public int CommandRead(List<string> menuOptions)
        {
            var commandsKeys = new StringBuilder();
            for (int i = 0; i < menuOptions.Count; i++)
            {
                commandsKeys.Append(i);
            }
            var regPattern = new Regex($"^[{commandsKeys?.ToString()}]{{1}}$");

            StringBuilder menuCommands = new StringBuilder();
            menuCommands.Append($"\nPlease choose your action: \n");
            foreach (var item in menuOptions)
            {
                menuCommands.Append(item);
            }
            var menu = menuCommands.ToString();
            Console.WriteLine(menu);
            Console.Write("\nAction: ");
            string? inputCommand = Console.ReadLine();
            while (!regPattern.IsMatch(inputCommand ?? string.Empty))
            {
                Console.WriteLine("\nERROR: Please input correct command from menu. (press any key to continue..)");
                var inputKey = Console.ReadKey();
                MoveToPreviousLine(inputKey, menu.Split("\n").Length + 4);
                Console.WriteLine(menu);
                Console.Write("\nAction: ");
                inputCommand = Console.ReadLine();
            }
            Console.WriteLine();
            return Convert.ToInt32(inputCommand);
        }

        public void ShowLogo()
        {
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine("| FFFFFFFFF                                BBBBBBBB                               |");
            Console.WriteLine("| FFFFFFFFF                                BBBBBBBBB                              |");
            Console.WriteLine("| FFF          AAA    SSSSSSSSS TTTTTTTTT  BBB   BBB    AAA    NNN   NNN KKK   KKK|");
            Console.WriteLine("| FFF        AAAAAAA  SSSSSSSSS TTTTTTTTT  BBB   BBB  AAAAAAA  NNNN  NNN KKK  KKK |");
            Console.WriteLine("| FFFFFFFFF AAA   AAA SSS          TTT     BBBBBBBBB AAA   AAA NNNNN NNN KKK KKK  |");
            Console.WriteLine("| FFFFFFFFF AAA   AAA SSSSSSSS     TTT     BBBBBBBBB AAA   AAA NNNNNNNNN KKKKKK   |");
            Console.WriteLine("| FFF       AAAAAAAAA  SSSSSSSS    TTT     BBB   BBB AAAAAAAAA NNN NNNNN KKKKKK   |");
            Console.WriteLine("| FFF       AAAAAAAAA       SSS    TTT     BBB   BBB AAAAAAAAA NNN  NNNN KKK KKK  |");
            Console.WriteLine("| FFF       AAA   AAA SSSSSSSSS    TTT     BBBBBBBBB AAA   AAA NNN   NNN KKK  KKK |");
            Console.WriteLine("| FFF       AAA   AAA SSSSSSSSS    TTT     BBBBBBBB  AAA   AAA NNN   NNN KKK   KKK|");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine();
        }

        public List<string> ValidateEmail(string email, List<string> validationErrors)
        {
            var regEmailPattern = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            if (!regEmailPattern.IsMatch(email))
            {
                validationErrors.Add("You entered wrong email");
            }
            return validationErrors;
        }

        public string InputEmail(string inquiryMsg = "Please input you email: ", string emailTypeToInput = "Email: ")
        {
            Console.WriteLine(inquiryMsg);
            Console.Write(emailTypeToInput);
            var email = Console.ReadLine();
            while (ValidateEmail(email ?? string.Empty, new List<string>()).Count > 0)
            {
                Console.WriteLine("You've inputted wrong email. Press any key to try again or type \"quit\" for exit");
                var keyIsEnter = Console.ReadKey();
                new MenuService().MoveToPreviousLine(keyIsEnter, 2);
                Console.Write(emailTypeToInput);
                email = Console.ReadLine()??string.Empty;
                if (email == "quit")
                return email;
            }
            return email ?? string.Empty;
        }

        public void OperationCompleteScreen()
        {
            Console.WriteLine("\nOperation completed successfully! Press any key to continue...");
            Console.ReadLine();
        }
    }
}
