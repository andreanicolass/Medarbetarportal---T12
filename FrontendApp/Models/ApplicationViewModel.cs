namespace FrontendApp.Models;

public class ApplicationViewModel
{
    public int Id { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public int BenefitId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? DecisionDate { get; set; }
}