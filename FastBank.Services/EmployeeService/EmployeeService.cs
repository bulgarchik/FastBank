using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace FastBank.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMenuService _menuService;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService()
        {
            _menuService = new MenuService();
            _employeeRepository = new EmployeeRepository();
        }

        public void ShowEmployeesMenu()
        {
            int currentPage = 1;

            List<Employee> employees;

            var employeesCount = _employeeRepository.GetEmployeeCount();

            int totalPages = (int)Math.Ceiling((double)employeesCount / EmployeeRepository.EMPLOYEES_PER_PAGE);

            do
            {
                Console.Clear();
                _menuService.ShowLogo();

                employees = _employeeRepository.GetEmployees(currentPage);

                if (employees != null && employees.Count() > 0)
                {
                    Console.WriteLine("Employees:");

                    Console.WriteLine("{0,-4} {1,-40} {2,-30}",
                       "| ID",
                       "| Name",
                       "| Role");
                    Console.WriteLine(new string('-', 85));

                    foreach (var employee in employees)
                    {
                        Console.WriteLine("{0,-4} {1,-40} {2,-30}",
                            $"| {employee.Index}",
                            $"| {employee.Name}",
                            $"| {employee.Role}");
                    }

                    Console.WriteLine($"\nPage {currentPage}/{totalPages}\n");
                }

                var menuOptions = $"\nPlease choose your action: \n" +
                               $"\n 1: For next page" +
                               $"\n 2: For previous page" +
                               $"\n 3: Add employee" +
                               $"\n 4: Terminate employee" +
                               $"\n 0: Exit";
                var commandsCount = 5;
                int action = _menuService.CommandRead(commandsCount, menuOptions);

                switch (action)
                {
                    case 1:
                        {
                            if (currentPage < totalPages)
                            {
                                currentPage++;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (currentPage > 1)
                            {
                                currentPage--;
                            }
                            break;
                        }
                    case 3:
                        {
                            var employee = AddEmployee();
                            if (employee != null)
                            {
                                employeesCount = _employeeRepository.GetEmployeeCount();
                                totalPages = (int)Math.Ceiling((double)employeesCount / EmployeeRepository.EMPLOYEES_PER_PAGE);
                            }
                            break;
                        }
                    case 4:
                        {
                            if (employees != null)
                            {
                                if (TerminateEmployee(employees))
                                {
                                    employeesCount = _employeeRepository.GetEmployeeCount();
                                    totalPages = (int)Math.Ceiling((double)employeesCount / EmployeeRepository.EMPLOYEES_PER_PAGE);
                                }
                            }
                            break;
                        }
                    case 0:
                        {
                            return;
                        }
                }
            }
            while (true);
        }

        public Employee? AddEmployee()
        {
            Employee? employee = null;
            Console.WriteLine("New employee adding process is started...\n");
            Console.WriteLine("Please input employee name:");
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? string.Empty;

            StringBuilder menuOptions = new StringBuilder($"\nPlease select employee role ID from the list: \n");
            var enums = Enum.GetValues(typeof(Role));
            foreach (var item in enums)
            {
                menuOptions.Append($"\n{(int)item}: {((Role)item).GetDisplayName()}");
            }

            int chosenRole = _menuService.CommandRead(enums.Length + 1, menuOptions.ToString(), 1);

            Console.WriteLine($"You will аdd new еmployee with name: {name} and role: {(Role)chosenRole}");
            Console.Write("\nPlease confirm with Y key:");
            var confirmKey = Console.ReadKey();
            if (confirmKey.Key == ConsoleKey.Y)
            {
                employee = new Employee(Guid.NewGuid(), name, null, (Role)chosenRole);
                _employeeRepository.AddEmployee(employee);
            }
            return employee;
        }

        public bool TerminateEmployee(List<Employee> employees)
        {
            Console.WriteLine("\nEmployee termination process is started...\n");

            var firstIndex = employees?.First()?.Index;
            var lastIndex = employees?.Last().Index;

            int employeeId;
            do
            {
                Console.WriteLine("Please enter employee ID (type 'q' for exit):");
                Console.Write("Employee ID: ");
                var inputEmploeyyId = Console.ReadLine() ?? null;

                if (inputEmploeyyId == "q")
                    return false;

                if (!int.TryParse(inputEmploeyyId, out employeeId) || employeeId < firstIndex || employeeId > lastIndex)
                {
                    Console.WriteLine("Please input correct employee ID (press any key to continue...)");
                    var keyIsEnter = Console.ReadKey();
                    new MenuService().MoveToPreviousLine(keyIsEnter, 3);
                }
            } while (employeeId < firstIndex || employeeId > lastIndex);

            var employeeToTerminate = employees?.Where(x => x.Index == employeeId).FirstOrDefault();
            if (employeeToTerminate != null)
            {
               return _employeeRepository.DeleteEmployee(employeeToTerminate.EmployeeId);
            }
            return false;
        }

    }
}
