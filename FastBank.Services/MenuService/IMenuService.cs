using System.Text.RegularExpressions;

namespace FastBank.Services
{
    public interface IMenuService
    {
        public string PasswordStaredInput();

        public void MoveToPreviousLine(ConsoleKeyInfo inputkey, int countOfLines = 1);

        public int CommandRead(Regex regPattern, string menuOptions);

        public void Logo();

        public string InputEmail();
    }
}
