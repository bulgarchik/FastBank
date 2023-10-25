using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.Infrastructure.DTOs
{
    [Table("Users")]
    public class UserDTO
    {
        private UserDTO() { }

        public UserDTO(User customer)
        {
            CustomerId = customer.Id;
            Name = customer.Name;
            Email = customer.Email;
            Birthday = customer.Birthday;
            Password = customer.Password;
            Role = customer.Role.ToString();
            Inactive = customer.Inactive;
        }

        [Key]
        public Guid CustomerId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime Birthday { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }
        public bool Inactive { get; private set; }

        public User ToDomainObj()
        {
            var role = Enum.Parse<Roles>(Role);

            return new User(CustomerId, Name, Email, Birthday, Password, role, Inactive);
        }
    }
}
