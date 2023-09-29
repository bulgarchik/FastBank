using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Domain.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAll();

        public void Add(Customer customer);
    }
}
