namespace FastBank
{
    public class Customer
    {
        public Customer(Guid id, string name, string email, DateTime birthday, string password, Roles role, bool inActive)
        {
            Id = id;
            Name = name;
            Email = email;
            Birthday = birthday;
            Password = password;
            Role = role;
            InActive = inActive;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime Birthday { get; private set; }
        public string Password { get; private set; }
        public Roles Role { get; private set; }
        public bool InActive { get; private set; }
    }
}
