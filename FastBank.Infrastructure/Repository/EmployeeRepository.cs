using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Context;
using FastBank.Infrastructure.DTOs;

namespace FastBank.Infrastructure.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IRepository _repo;

        public const int EmployeesPerPage = 3;

        public EmployeeRepository()
        {
            _repo = new Repository(new FastBankDbContext());
        }
        public void AddEmployee(Employee employee)
        {
            _repo.Add(new EmployeeDTO(employee));
        }

        public bool DeleteEmployee(Guid employeeId)
        {
            var employeeToDelete = _repo.Set<EmployeeDTO>().Where(e => e.EmployeeId == employeeId).FirstOrDefault();
            if (employeeToDelete != null)
            {
                _repo.Delete<EmployeeDTO>(employeeToDelete);
                return true;
            }
            
            return false;
        }

        public List<Employee> GetEmployees(int currentPage = 1)
        {
            var currentPageEmployeeIndex = (currentPage - 1) * EmployeesPerPage + 1;

            var employees = _repo.Set<EmployeeDTO>()
                                 .OrderBy(e => e.Name).ThenBy(e => e.EmployeeId)
                                 .Skip((currentPage - 1) * EmployeesPerPage)
                                 .Take(EmployeesPerPage)
                                 .Select(e => e.ToDomeinObj())
                                 .ToList();

            foreach (var item in employees)
            {
                item.Index = currentPageEmployeeIndex++;
            }

            return employees;
        }

        public int GetEmployeeCount()
        {
            return _repo.Set<EmployeeDTO>().Count();
        }
    }
}
