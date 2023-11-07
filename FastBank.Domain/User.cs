namespace FastBank
{
    public class User
    {
        public User(Guid id, string name, string email, DateTime birthday, string password, Role role, bool inactive)
        {
            Id = id;
            Name = name;
            Email = email;
            Birthday = birthday;
            Password = password;
            Role = role;
            Inactive = inactive;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime Birthday { get; private set; }
        public string Password { get; private set; }
        public Role Role { get; private set; }
        public bool Inactive { get; private set; }
    }
}
