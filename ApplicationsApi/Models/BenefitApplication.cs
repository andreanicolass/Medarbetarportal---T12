using System.ComponentModel.DataAnnotations;

namespace ApplicationsApi.Models;

public class BenefitApplication
{
    public int Id { get; set; }
    [MaxLength(50)]
    public required string EmployeeId { get; set; } 
    public int BenefitId { get; set; }
    [MaxLength(50)]
    public required string Status { get; set; } 
    public DateTime CreatedDate { get; set; }
    public DateTime? DecisionDate  { get; set; }
}