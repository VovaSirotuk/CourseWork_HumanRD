using Xunit.Sdk;

namespace CompanyApp.Tests
{
    public class EmployeeTest
    {
        private readonly Company _company;

        static List<Employee> ParseEmployees()
        {
            using Stream st = File.OpenRead(Path.Combine("..", "..", "..", "Data", "employees.json"));
            return JsonSerializer.Deserialize<List<Employee>>(st);
        }

        static List<Division> ParseDivisions()
        {
            using Stream st = File.OpenRead(Path.Combine("..", "..", "..", "Data", "divisions.json"));
            return JsonSerializer.Deserialize<List<Division>>(st);
        }

        static List<EmployeeProject> ParseEmployeesProjects()
        {
            using Stream st = File.OpenRead(Path.Combine("..", "..", "..", "Data", "employeesprojects.json"));
            return JsonSerializer.Deserialize<List<EmployeeProject>>(st);
        }

        static List<Position> ParsePositions()
        {
            using Stream st = File.OpenRead(Path.Combine("..", "..", "..", "Data", "positions.json"));
            return JsonSerializer.Deserialize<List<Position>>(st);
        }

        static List<Project> ParseProjects()
        {
            using Stream st = File.OpenRead(Path.Combine("..", "..", "..", "Data", "projects.json"));
            return JsonSerializer.Deserialize<List<Project>>(st);
        }


        public EmployeeTest()
        {
            _company = new(ParseDivisions(), ParseEmployees(), ParsePositions(), ParseProjects(), ParseEmployeesProjects());
        }

        [Fact]
        public void AddEmployee_CollectionContainsEmployee()
        {
            Employee newEmployee = new()
            {
                Id = 15,
                FirstName = "Lunar",
                LastName = "Kikavili",
                Experience = 10,
                PositionId = 7,
                DivisionId = 2
            };

            _company.AddEmployee(newEmployee.FirstName, newEmployee.LastName, newEmployee.Experience, "Team Lead", "Developer");


            Assert.DoesNotContain(newEmployee, _company.Employees);
        }


        [Fact]
        public void RemoveEmployee_CollectionDoesNotContainEmployee()
        {
            Employee employee = new()
            {
                Id = 2,
                Experience = 3,
                FirstName = "Marichka",
                LastName = "Courtois",
                PositionId = 3,
                DivisionId = 2
            };

            _company.RemoveEmployee(employee.FirstName, employee.LastName);

            Assert.DoesNotContain(employee, _company.Employees);
        }
    }
}