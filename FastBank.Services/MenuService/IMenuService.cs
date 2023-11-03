using System.Text.RegularExpressions;

namespace FastBank.Services
{
    public interface IMenuService
    {
        public string PasswordStaredInput();

        public void MoveToPreviousLine(ConsoleKeyInfo inputkey, int countOfLines = 1);

        public int CommandRead(Regex regPattern, string menuOptions);

        public void ShowLogo();

        public List<string> ValidateEmail(string email, List<string> validationErrors);

        public string InputEmail(string inquiryMsg, string emailTypeToInput);
    }
}
