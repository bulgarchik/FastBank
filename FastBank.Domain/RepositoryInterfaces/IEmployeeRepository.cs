namespace FastBank.Domain.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        public void AddEmployee(Employee employee);

        public bool DeleteEmployee(Guid employeeId);

        public List<Employee> GetEmployees(int currentPage = 1);

        public int GetEmployeeCount();
    }
}
