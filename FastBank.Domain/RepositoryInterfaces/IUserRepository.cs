namespace FastBank.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        public List<User> GetAll();

        public User? GetByEmail(string email);

        public void Add(User user);

        public List<User> GetUserFriends(User user);

        public void AddFriend(User user, User friend);

        public void RemoveFriend(User user, User friend);
    }
}
