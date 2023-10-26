using System.Data;
using System.Xml.Linq;

namespace FastBank
{
    public class Customer : User
    {
        public Customer(Guid id, string name, string email, DateTime birthday, string password, Roles role, bool inactive)
                : base(id, name, email, birthday, password, role, inactive) { }
        
        public Customer(User user) : base(
                                       user.Id,
                                       user.Name,
                                       user.Email,
                                       user.Birthday,
                                       user.Password,
                                       user.Role,
                                       user.Inactive)
        {

        }
    }
}
