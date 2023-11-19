using FastBank.Domain;
using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;
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

            int totalPages = (int)Math.Ceiling((double)employeesCount / EmployeeRepository.EmployeesPerPage);

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
                               $"\n 3: Add employes" +
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
                                totalPages = (int)Math.Ceiling((double)employeesCount / EmployeeRepository.EmployeesPerPage);
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
                                    totalPages = (int)Math.Ceiling((double)employeesCount / EmployeeRepository.EmployeesPerPage);
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
            Console.WriteLine("\nNew employee adding process is started...\n");
            Console.WriteLine("Please input employee name:");
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? string.Empty;

            StringBuilder menuOptions = new StringBuilder($"Please select employee role ID from the list: \n");
            var enums = Enum.GetValues(typeof(Role));
            foreach (var item in enums)
            {
                menuOptions.Append($"{(int)item}: {item.ToString()}\n");
            }

            int chosenRole = _menuService.CommandRead(enums.Length, menuOptions.ToString());

            Console.WriteLine($"\n You will Add new Employee with name: {name} and role: {(Role)chosenRole}");
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

            int employeeId;
            do
            {
                Console.WriteLine("Please enter employee ID (type 'q' for exit):");
                Console.Write("Employee ID: ");
                var inputEmploeyyId = Console.ReadLine() ?? null;

                if (inputEmploeyyId == "q")
                    return false;

                if (!int.TryParse(inputEmploeyyId, out employeeId) || employeeId < employees.First()?.Index || employeeId > employees.Last().Index)
                {
                    Console.WriteLine("Please input correct employee ID (press any key to continue...)");
                    var keyIsEnter = Console.ReadKey();
                    new MenuService().MoveToPreviousLine(keyIsEnter, 3);
                }
            } while (employeeId < employees.First()?.Index || employeeId > employees.Last().Index);

            var employeeToTerminate = employees.Where(x => x.Index == employeeId).FirstOrDefault();
            if (employeeToTerminate != null)
            {
               return _employeeRepository.DeleteEmployee(employeeToTerminate.EmployeeId);
            }
            return false;
        }

    }
}
