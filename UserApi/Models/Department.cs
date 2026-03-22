namespace UserApi.Models;

public class Department
{
    public int Id { get; set; }
    
    public required string Name { get; set; }

    public List<User> Users { get; set; } = new();
}