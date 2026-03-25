using Medarbetarportal.Web.Models; 
using Microsoft.AspNetCore.Mvc;

namespace Medarbetarportal.Web.Controllers;

public class DashboardController : Controller
{
    private readonly HttpClient _httpClient;

    public DashboardController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<IActionResult> Index()
    {
        var apiAddress = "http://localhost:5237/api/Benefits";
    
        try 
        {
            
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
            var benefits = await _httpClient.GetFromJsonAsync<List<Benefit>>(apiAddress, options);
            return View(benefits);
        }
        catch 
        {
            return View(new List<Benefit>());
        }
    }
}