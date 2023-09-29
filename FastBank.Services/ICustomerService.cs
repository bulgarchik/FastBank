using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Services
{
    public interface ICustomerService
    {
        public List<Customer> GetAll();

        public void Add(Customer customer);

        public void Add(string name, string email, DateTime birthday, string password, Roles role);
    }
}
