namespace Benefits_Api.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // Relation: En kategori kan ha många förmåner
    public List<Benefit> Benefits { get; set; } = new();
}