using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.Infrastructure.DTOs
{
    [Table("Users")]
    public class UserDTO
    {
        public UserDTO() { }

        public UserDTO(User user)
        {
            UserId = user.Id;
            Name = user.Name;
            Email = user.Email;
            Birthday = user.Birthday;
            Password = user.Password;
            Role = user.Role.ToString();
            Inactive = user.Inactive;
        }
        public UserDTO(Guid id, string name, string email, DateTime birthday, string password, Roles role, bool inactive)
        {
            UserId = id;
            Name = name;
            Email = email;
            Birthday = birthday;
            Password = password;
            Role = role.ToString();
            Inactive = inactive;
        }

        [Key]
        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime Birthday { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }
        public bool Inactive { get; private set; }

        public User ToDomainObj()
        {
            var role = Enum.Parse<Roles>(Role);

            return new User(UserId, Name, Email, Birthday, Password, role, Inactive);
        }
    }
}
