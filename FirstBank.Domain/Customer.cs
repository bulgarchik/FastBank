using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank
{
    public class Customer
    {
        public Customer(Guid id, string name, string email, DateTime birthday, string password, Roles role)
        {
            Id = id;
            Name = name;
            Email = email;
            Birthday = birthday;
            Password = password;
            Role = role;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime Birthday { get; private set; }
        public string Password { get; private set; }
        public Roles Role { get; private set; }
    }
}
