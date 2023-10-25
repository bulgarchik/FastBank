namespace FastBank
{
    public class Customer : User
    {
        public Customer(Guid id, string name, string email, DateTime birthday, string password, Roles role, bool inactive)
                : base(id, name, email, birthday, password, role, inactive) { }
    }
}
