using System.ComponentModel.DataAnnotations;

namespace FastBank
{
    public enum Role
    {
        Accountant = 1,
        Manager,
        Customer,
        Banker,

        [Display(Name = "Customer service")]
        CustomerService,
    }
}
