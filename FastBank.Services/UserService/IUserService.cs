namespace FastBank.Services
{
    public interface IUserService
    {
        public List<User> GetAll();

        public void Add(User customer);

        public void Add(string name, string email, DateTime birthday, string password, Roles role, bool inactive);

        public List<string> CheckLoginUserName(string username);

        public User? Login(string username, string password);

        public List<User> GetUserFriends(User user);

        public void AddFriend(User user, List<User> friendsList);

        public void RemoveFriend(User user);
    }
}
