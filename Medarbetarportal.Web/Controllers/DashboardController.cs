using Medarbetarportal.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Medarbetarportal.Web.Controllers;

public class DashboardController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl = "http://localhost:5237/api/Benefits";

    public DashboardController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }
    
    public async Task<IActionResult> Index()
    {
        try 
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var benefits = await _httpClient.GetFromJsonAsync<List<Benefit>>(_apiBaseUrl, options);
            
            return View(benefits);
        }
        catch 
        {
            return View(new List<Benefit>());
        }
    }
    
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            var benefit = await _httpClient.GetFromJsonAsync<Benefit>($"{_apiBaseUrl}/{id}", options);

            if (benefit == null)
            {
                return NotFound();
            }

            return View(benefit);
        }
        catch
        {
            return RedirectToAction(nameof(Index));
        }
    }
}