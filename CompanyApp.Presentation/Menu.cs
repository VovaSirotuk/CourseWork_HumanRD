using CompanyApp.BusinessLogic;
using CompanyApp.DataAccess;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace CompanyApp.Presentation;

public class Menu
{
    Serializer<List<Division>> divisionsSerializer = new(Path.Combine("..", "..", "..", "Data", "divisions.json"));
    Serializer<List<Employee>> employeesSerializer = new(Path.Combine("..", "..", "..", "Data", "employees.json"));
    Serializer<List<Position>> positionsSerializer = new(Path.Combine("..", "..", "..", "Data", "positions.json"));
    Serializer<List<Project>> projectsSerializer = new(Path.Combine("..", "..", "..", "Data", "projects.json"));
    Serializer<List<EmployeeProject>> employeesProjectsSerializer = new(Path.Combine("..", "..", "..", "Data", "employeesprojects.json"));

    string firstName;
    string lastName;
    string title;
    string command;
    Company company;
    private static void PrintHelp()
    {
        Console.WriteLine();
        Console.WriteLine("1 Employee");
        Console.WriteLine("2 Division");
        Console.WriteLine("3 Position");
        Console.WriteLine("4 Find");        
        Console.WriteLine("0 Quit");
    }

    public void Run()
    {

       company = new(
            divisionsSerializer.Load(),
            employeesSerializer.Load(),
            positionsSerializer.Load(),
            projectsSerializer.Load(),
            employeesProjectsSerializer.Load()
        );
             
            PrintHelp();

            Console.Write("Action: ");
            command = Console.ReadLine();
        switch (command)
        {
            case "1":
                Employee();
                break;
            case "2":
                Division();
                break;
            case "3":
                Position();
                break;
            case "4":
                Find();
                break;

            default:
                Console.WriteLine();
                Console.WriteLine("Command Not Recognized \nTry again");
                Run();
                Console.WriteLine();
                break;
        }        
    }
    private void Employee()
    {
        Console.WriteLine("1 Add Employee");
        Console.WriteLine("2 Remove Employee");
        Console.WriteLine("3 Update Employee Data");
        Console.WriteLine("4 Print Employee Data");
        Console.WriteLine("5 Print Employee Projects");
        Console.WriteLine("6 Print All Employees");
        Console.WriteLine("0 Back");

        Console.Write("Action: ");
        command = Console.ReadLine();

        switch (command)
        {

            case "1":
                Console.Write("First Name: ");
                firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                lastName = Console.ReadLine();

                Console.Write("Experience: ");
                int experience = int.Parse(Console.ReadLine());

                Console.Write("Position: ");
                string positionTitle = Console.ReadLine();

                Console.Write("Division: ");
                string divisionTitle = Console.ReadLine();

                try { company.AddEmployee(firstName, lastName, experience, positionTitle, divisionTitle); Console.WriteLine("Success"); }
                catch { Console.WriteLine("Something Went Wrong"); }
                employeesSerializer.Save(company.Employees);
                break;

            case "2":
                Console.Write("First Name: ");
                firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                lastName = Console.ReadLine();

                try { company.RemoveEmployee(firstName, lastName); Console.WriteLine("Success"); }
                catch { Console.WriteLine("Something Went Wrong"); }
                employeesSerializer.Save(company.Employees);

                break;

            case "3":
                Console.WriteLine("All Data Except First Name and Last Name Will Be Changed");
                Console.Write("First Name: ");
                firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                lastName = Console.ReadLine();

                Console.Write("New Experience: ");
                experience = int.Parse(Console.ReadLine());

                Console.Write("New Position: ");
                positionTitle = Console.ReadLine();

                Console.Write("New Division: ");
                divisionTitle = Console.ReadLine();

                try { company.UpdateEmployeeData(firstName, lastName, experience, positionTitle, divisionTitle); Console.WriteLine("Success"); }
                catch { Console.WriteLine("Something Went Wrong"); }
                employeesSerializer.Save(company.Employees);

                break;

            case "4":
                Console.Write("First Name: ");
                firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                lastName = Console.ReadLine();

                try { Console.WriteLine(company.GetEmployeeData(firstName, lastName)); }
                catch { Console.WriteLine("Something Went Wrong"); }
                break;


            case "5":
                Console.Write("First Name: ");
                firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                lastName = Console.ReadLine();

                try
                {
                    foreach (Project project in company.GetEmployeeProjects(firstName, lastName))
                    {
                        Console.WriteLine($"Title: {project.Title}");
                        Console.WriteLine($"Profit: {project.Profit}");
                        Console.WriteLine();
                    }
                }
                catch { Console.WriteLine("Something Went Wrong"); }
                break;

            case "6":
                Console.WriteLine("1 Print All Employees Ordered By First Name");
                Console.WriteLine("2 Print All Employees Ordered By Last Name");
                Console.WriteLine("3 Print All Employees Ordered By Salary");

                Console.Write("Action: ");
                command = Console.ReadLine();
                switch (command)
                {
                    case "1":
                        foreach (Employee employee in company.Employees.OrderBy(emp => emp.FirstName))
                            Console.WriteLine($"{employee.FirstName} {employee.LastName}, Salary: {company.Positions.First(pos => pos.Id == employee.PositionId).Salary}");
                        break;

                    case "2":
                        foreach (Employee employee in company.Employees.OrderBy(emp => emp.LastName))
                            Console.WriteLine($"{employee.FirstName} {employee.LastName}, Salary: {company.Positions.First(pos => pos.Id == employee.PositionId).Salary}");
                        break;

                    case "3":
                        foreach (Employee employee in company.Employees.OrderBy(emp => company.Positions.First(pos => pos.Id == emp.PositionId).Salary))
                            Console.WriteLine($"{employee.FirstName} {employee.LastName}, Salary: {company.Positions.First(pos => pos.Id == employee.PositionId).Salary}");
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Command Not Recognized");
                        Run();
                        Console.WriteLine();
                        break;
                }
                break;
            case "0":
                Run();
                break;
            default:
                Console.WriteLine();
                Console.WriteLine("Command Not Recognized\nTry again");
                Console.WriteLine();
                break;
        }
        Run();
    }
    private void Division()
    {
        Console.WriteLine("1 Update Division Data");
        Console.WriteLine("2 Add Division");
        Console.WriteLine("3 Print Division Data");
        Console.WriteLine("4 Print Division Employees Ordered By Position");
        Console.WriteLine("5 Print Division Employees Ordered By Participated Projects Profit Sum");
        Console.WriteLine("0 Back");

        Console.Write("Action: ");
        command = Console.ReadLine();

        switch (command)
        {



            case "1":

                Console.Write("Division Title: ");
                title = Console.ReadLine();

                Console.Write("New Division Title: ");
                string newTitle = Console.ReadLine();

                try { company.UpdateDivisionData(title, newTitle); }
                catch { Console.WriteLine("Something Went Wrong"); }
                divisionsSerializer.Save(company.Divisions);

                break;

            case "2":

                Console.Write("Division Title: ");
                title = Console.ReadLine();

                company.AddDivision(title);
                Console.WriteLine("Success");
                divisionsSerializer.Save(company.Divisions);

                break;

            case "3":

                Console.Write("Division Title: ");
                title = Console.ReadLine();

                try { Console.WriteLine(company.GetDivisionData(title).Title); }
                catch { Console.WriteLine("Something Went Wrong"); }
                break;

            case "4":

                Console.Write("Division Title: ");
                title = Console.ReadLine();

                foreach (Employee employee in company.GetDivisionEmployeesByPosition(title))
                {
                    Console.WriteLine($"First Name: {employee.FirstName}");
                    Console.WriteLine($"Last Name: {employee.LastName}");
                    Console.WriteLine($"Experience: {employee.Experience}");
                }
                break;
            case "5":

                Console.Write("Division Title: ");
                title = Console.ReadLine();

                foreach (Employee employee in company.GetDivisionEmployeesByProjectsCount(title))
                {
                    Console.WriteLine($"First Name: {employee.FirstName}");
                    Console.WriteLine($"Last Name: {employee.LastName}");
                    Console.WriteLine($"Experience: {employee.Experience}");
                }
                break;
            case "0":
                Run();
                break;
            default:
                Console.WriteLine();
                Console.WriteLine("Command Not Recognized\nTry again");
                Run();
                Console.WriteLine();
                break;
        }
        Run();
    }
    private void Position()
    {

        Console.WriteLine("1 Update Position Data");
        Console.WriteLine("2 Print 5 Most Attractive Positions");
        Console.WriteLine("3 Print Most Profitable Employee With Given Position");
        Console.WriteLine("0 Back");

        Console.Write("Action: ");
        command = Console.ReadLine();

        switch (command)
        {
            case "1":
                Console.Write("Position Title: ");
                title = Console.ReadLine();

                Console.Write("New Salary: ");
                decimal newSalary = decimal.Parse(Console.ReadLine());

                Console.Write("New Working Hours: ");
                int newWorkingHours = int.Parse(Console.ReadLine());

                company.UpdatePositionData(title, newSalary, newWorkingHours);
                positionsSerializer.Save(company.Positions);
                break;

            case "2":
                foreach (Position position in company.Get5MostAttractivePositions())
                    Console.WriteLine($"Title: {position.Title}; Salary: {position.Salary}; Working Hours: {position.WorkingHours}");
                break;

            case "3":
                Employee mostProfitableEmployee = company.GetMostProfitableEmployee();

                Console.WriteLine($"First Name: {mostProfitableEmployee.FirstName}");
                Console.WriteLine($"Last Name{mostProfitableEmployee.LastName}");
                Console.WriteLine($"Experience: {mostProfitableEmployee.Experience}");
                break;
            case "0":
                Run();
                break;
            default:
                Console.WriteLine();
                Console.WriteLine("Command Not Recognized\nTry again");
                Console.WriteLine();
                break;
        }
    }
    private void Find()
    {

        Console.WriteLine("1 Find Employee By Keyword");
        Console.WriteLine("2 Find Project By Keyword");
        Console.WriteLine("3 Find Anything By Keyword");
        Console.WriteLine("4 Find Employee By Attributes");
        Console.WriteLine("0 Back");

        Console.Write("Action: ");
        command = Console.ReadLine();

        switch (command)
        {
            case "1":
                Console.Write("Keyword: ");
                string keyword = Console.ReadLine();

                foreach (Employee employee in company.GetEmployeesByKeyword(keyword))
                {
                    Console.WriteLine($"First Name: {employee.FirstName}");
                    Console.WriteLine($"Last Name: {employee.LastName}");
                    Console.WriteLine($"Experience: {employee.Experience}");
                    Console.WriteLine();
                }
                break;

            case "2":
                Console.Write("Keyword: ");
                keyword = Console.ReadLine();

                foreach (Project project in company.GetProjectsByKeyword(keyword))
                {
                    Console.WriteLine($"Title: {project.Title}");
                    Console.WriteLine($"Profit: {project.Profit}");
                    Console.WriteLine();
                }
                break;

            case "3":
                Console.Write("Keyword: ");
                keyword = Console.ReadLine();

                IEnumerable<Project> projects = company.GetProjectsByKeyword(keyword);
                IEnumerable<Division> divisions = company.GetDivisionsByKeyword(keyword);

                Console.WriteLine("Employees By Keyword: ");
                foreach (Employee employee in company.GetEmployeesByKeyword(keyword))
                {
                    Console.WriteLine($"First Name: {employee.FirstName}");
                    Console.WriteLine($"Last Name: {employee.LastName}");
                    Console.WriteLine($"Experience: {employee.Experience}");
                    Console.WriteLine();
                }

                Console.WriteLine("Projects By Keyword: ");
                foreach (Division division in company.GetDivisionsByKeyword(keyword))
                {
                    Console.WriteLine($"First Name: {division.Title}");
                    Console.WriteLine();
                }

                break;

            case "4":
                Console.Write("First Name: ");
                firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                lastName = Console.ReadLine();

                Employee foundEmployee = company.GetEmployeeByName(firstName, lastName);

                Console.WriteLine($"First Name: {foundEmployee.FirstName}");
                Console.WriteLine($"Last Name: {foundEmployee.LastName}");
                Console.WriteLine($"Experience: {foundEmployee.Experience}");
                Console.WriteLine();
                break;
            case "0":
                Run();
                break;
            default:
                Console.WriteLine();
                Console.WriteLine("Command Not Recognized\nTry again");
                Run();
                Console.WriteLine();
                break;
        }
        Run();
    }
    
}
