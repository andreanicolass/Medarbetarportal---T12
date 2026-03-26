namespace Benefits_Api.Models;

public class Benefit
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; } 
    public int Price { get; set; }           
    public bool IsActive { get; set; }
    
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}