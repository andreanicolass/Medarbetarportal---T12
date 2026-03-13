using System.ComponentModel.DataAnnotations;

namespace UserApi.Models;

public class User
{
    public int Id { get; set; }

    [MaxLength(50)]
    public required string FirstName { get; set; }

    [MaxLength(50)]
    public required string LastName { get; set; }

    [MaxLength(100)]
    public required string Email { get; set; }

    [MaxLength(50)]
    public required string EmployeeId { get; set; }
    
    public int DepartmentId { get; set; }
    
    public Department? Department { get; set; }
}
