using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Infrastructure.DTOs
{
    public class CustomerDTO : UserDTO
    {
        public CustomerDTO(User customer) : base(customer)
        {
            
        }

        public new Customer ToDomainObj()
        {
            var role = Enum.Parse<Roles>(Role);

            return new Customer(CustomerId, Name, Email, Birthday, Password, role, Inactive);
        }
    }
}
