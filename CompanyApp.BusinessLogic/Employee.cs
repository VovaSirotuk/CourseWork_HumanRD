namespace CompanyApp.DataAccess;

public class Employee
{
    public int Id { get; set; }

    public int Experience { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public int PositionId { get; set; }
    public int DivisionId { get; set; }
}
