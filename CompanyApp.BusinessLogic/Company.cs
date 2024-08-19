using CompanyApp.DataAccess;

namespace CompanyApp.BusinessLogic;

public class Company
{
    public List<Division> Divisions { get; set; }
    public List<Employee> Employees { get; set; }
    public List<Position> Positions { get; set; }
    public List<Project> Projects { get; set; }
    public List<EmployeeProject> EmployeesProjects { get; set; }

    public Company(
        IEnumerable<Division> divisions, 
        IEnumerable<Employee> employees,
        IEnumerable<Position> positions,
        IEnumerable<Project> projects,
        IEnumerable<EmployeeProject> employeesProjects
        )
    {
        Divisions = divisions.ToList();
        Employees = employees.ToList();
        Positions = positions.ToList();
        Projects = projects.ToList();
        EmployeesProjects = employeesProjects.ToList();
    }

    // 1.1
    public void AddEmployee(string firstName, string lastName, int experience, string position, string division)
    {
        if (!Positions.Any(pos => pos.Title == position))
            throw new ArgumentException();
        if (!Divisions.Any(div => div.Title == division))
            throw new ArgumentException();

        Employees.Add(new Employee
        {
            Id = Employees.Count + 1,
            FirstName = firstName,
            LastName = lastName,
            Experience = experience,
            PositionId = Positions.First(pos => pos.Title == position).Id,
            DivisionId = Divisions.First(div => div.Title == division).Id
        });
    }

    // 1.2
    public void RemoveEmployee(string firstName, string lastName)
    {
        Employee employee = Employees.FirstOrDefault(emp => emp.FirstName == firstName && emp.LastName == lastName);
        if (employee is not null)
            Employees.Remove(employee);
        else
            throw new ArgumentException();
    }

    // 1.3
    public void UpdateEmployeeData(string firstName, string lastName, int newExperience, string newPosition, string newDivision)
    {
        if (!Positions.Any(pos => pos.Title == newPosition))
            throw new ArgumentException();
        if (!Divisions.Any(div => div.Title == newDivision))
            throw new ArgumentException();

        Employee employee = Employees.FirstOrDefault(emp => emp.FirstName == firstName && emp.LastName == lastName);
        employee.Experience = newExperience;
        employee.PositionId = Positions.First(pos => pos.Title == newPosition).Id;
        employee.DivisionId = Divisions.First(div => div.Title == newDivision).Id;
    }

    // 1.4
    public string GetEmployeeData(string firstName, string lastName)
    {
        Employee employee = Employees.First(emp => emp.FirstName == firstName && emp.LastName == lastName);

        return "{" + Environment.NewLine +
            $"First Name: {employee.FirstName}" + Environment.NewLine +
            $"Last Name: {employee.LastName}" + Environment.NewLine +
            $"Experience: {employee.Experience}" + Environment.NewLine +
            $"Position: {Positions.First(pos => pos.Id == employee.PositionId).Title}" + Environment.NewLine +
            $"Division: {Divisions.First(div => div.Id == employee.DivisionId).Title}" + Environment.NewLine +
            "}";
    }

    // 1.5
    public IEnumerable<Project> GetEmployeeProjects(string firstName, string lastName)
    {
        Employee employee = Employees.First(emp => emp.FirstName == firstName && emp.LastName == lastName);
        IEnumerable<int> employeeProjectsIds = EmployeesProjects.Where(ep => ep.EmployeeId == employee.Id).Select(ep => ep.ProjectId);


        return Projects.Where(project => employeeProjectsIds.Contains(project.Id));
    }

    // 2.1
    public void UpdateDivisionData(string title, string newTitle)
    {
        Division division = Divisions.First(div => div.Title == title);

        division.Title = newTitle;
    }

    // 2.2
    public void AddDivision(string title)
    {
        Divisions.Add(new Division
        {
            Id = Divisions.Count + 1,
            Title = title
        });
    } 

    // 2.3
    public Division GetDivisionData(string title)
    {
        return Divisions.First(division => division.Title == title);
    }

    // 2.4.1
    public IEnumerable<Employee> GetDivisionEmployeesByPosition(string title)
    {
        Division division = Divisions.First(div => div.Title == title);
        return Employees.Where(emp => emp.DivisionId == division.Id).OrderBy(emp => Positions.First(pos => pos.Id == emp.Id).Title);
    }

    // 2.4.2
    public IEnumerable<Employee> GetDivisionEmployeesByProjectsCount(string title)
    {
        Division division = Divisions.First(div => div.Title == title);
        return Employees.Where(emp => emp.DivisionId == division.Id).OrderBy(emp => (EmployeesProjects.Where(ep => ep.EmployeeId == emp.Id).Count()));
    }


    // 3.1
    public void UpdatePositionData(string title, decimal newSalary, int newWorkingHours)
    {
        Position position = Positions.First(pos => pos.Title == title);
        position.Salary = newSalary;
        position.WorkingHours = newWorkingHours;
    }

    // 3.2
    public IEnumerable<Position> Get5MostAttractivePositions()
    {
        return Positions.OrderBy(pos => pos.WorkingHours / pos.Salary).Take(5);
    }

    
    // 3.3
    public Employee GetMostProfitableEmployee()
    {
        return Employees.OrderBy(
            emp => emp.Experience
            /
            Projects.Where(project => EmployeesProjects.Where(ep => ep.EmployeeId == emp.Id).Select(ep => ep.ProjectId).Contains(project.Id)).Select(project => project.Profit).Sum()
        ).First();
    }

    // 4.1
    public IEnumerable<Employee> GetEmployeesByKeyword(string keyword)
    {
        return Employees.Where(emp => 
            emp.FirstName.Contains(keyword)
            ||
            emp.LastName.Contains(keyword)
            ||
            emp.Experience.ToString().Contains(keyword)
        );
    }

    // 4.2
    public IEnumerable<Project> GetProjectsByKeyword(string keyword)
    {
        return Projects.Where(project =>
            project.Title.Contains(keyword)
            ||
            project.Profit.ToString().Contains(keyword)
        );
    }

    // 4.3
    public IEnumerable<Division> GetDivisionsByKeyword(string keyword)
    {
        return Divisions.Where(div => div.Title.Contains(keyword));
    }

    // 4.4
    public Employee GetEmployeeByName(string firstName, string lastName)
    {
        return Employees.FirstOrDefault(emp => emp.FirstName == firstName && emp.LastName == lastName);
    }


}
