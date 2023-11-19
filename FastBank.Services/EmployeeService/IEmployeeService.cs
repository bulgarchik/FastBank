using FastBank.Domain;

namespace FastBank.Services.EmployeeService
{
    public interface IEmployeeService
    {
        public Employee? AddEmployee();

        public bool TerminateEmployee(List<Employee> employees);

        public void ShowEmployeesMenu();
    }
}
