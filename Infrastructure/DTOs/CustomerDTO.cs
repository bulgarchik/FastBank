using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.Infrastructure.DTOs
{
    [Table("Customers")]
    public class CustomerDTO
    {
        private CustomerDTO() { }

        public CustomerDTO(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Email = customer.Email;
            Birthday = customer.Birthday;
            Password = customer.Password;
            Role = customer.Role.ToString();
            InActive = customer.InActive;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime Birthday { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }
        public bool InActive {  get; private set; }

        public Customer ToDomainObj()
        {
            var role = Enum.Parse<Roles>(Role);

            return new Customer(Id, Name, Email, Birthday, Password, role, InActive);
        }
    }
}
