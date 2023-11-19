using FastBank.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.Infrastructure.DTOs
{
    [Table("Emoyees")]
    public class EmployeeDTO
    {
        private EmployeeDTO() { }
        public EmployeeDTO(Employee employee)
        {
            EmployeeId = employee.EmployeeId;
            Name = employee.Name;
            UserId = employee.User?.Id;
            User = employee.User;
            Role = employee.Role;
        }

        [Key]
        public Guid EmployeeId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public Guid? UserId { get; private set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; private set; }
        public Role Role { get; private set; }

        public Employee ToDomeinObj()
        {
            return new Employee(EmployeeId, Name, User, Role);
        }
    }
}
