using System.Text.Json.Serialization;

namespace Medarbetarportal.Web.Models;

public class Benefit
{
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("price")]
    public int Price { get; set; }
    
    public bool IsActive { get; set; }
}