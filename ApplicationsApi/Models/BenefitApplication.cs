namespace ApplicationsApi.Models;

public class BenefitApplication
{
    public int Id { get; set; }
    public string EmployeeId { get; set; } 
    public int BenefitId { get; set; }
    public string Status { get; set; } 
    public DateTime CreatedDate { get; set; }
}