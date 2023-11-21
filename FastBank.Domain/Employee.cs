namespace FastBank.Domain
{
    public class Employee
    {
        public Employee(Guid id, string name, User? user, Role role)
        {
            EmployeeId = id;
            Name = name;
            User = user;
            Role = role;
        }

        public Guid EmployeeId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public User? User { get; private set; }
        public Role Role { get; private set; }
        public int Index { get; set; }

    }
}
