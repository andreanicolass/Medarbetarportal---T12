namespace Benefits_Api.Models;

public class Benefit
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty; 
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
    public bool IsActive { get; set; } = true;
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}