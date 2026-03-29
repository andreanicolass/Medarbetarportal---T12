namespace Benefits_Api.Models;

public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<Benefit> Benefits { get; set; } = new();
}