namespace FastBank.Services
{
    public interface IUserService
    {
        public List<User> GetAll();

        public void Add(User customer);

        public void Add(string name, string email, DateTime birthday, string password, Roles role, bool inactive);

        public List<string> CheckLoginUserName(string username);

        public User? Login(string username, string password);

        public List<string> ValidateEmail(string email, List<string> validationErrors);
    }
}
