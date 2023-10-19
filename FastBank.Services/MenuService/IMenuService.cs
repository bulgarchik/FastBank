namespace FastBank.Services
{
    public interface IMenuService
    {
        public string PasswordStaredInput();

        public void MoveToPreviousLine(int countOfLines = 1);
    }
}
