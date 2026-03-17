using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace StatistikAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public StatisticsController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet("total-applications")]
        public async Task<ActionResult<int>> GetTotalApplications()
        {
            try
            {
                // CHRISTOFFER - Applications API (fråga Christoffer om hans port!)
                // ÄNDRA DENNA TILL RÄTT PORT!
                var response = await _httpClient.GetStringAsync("http://localhost:5001/api/applications");
                var applications = JsonSerializer.Deserialize<List<object>>(response);
                return applications.Count;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fel vid hämtning av ansökningar: {ex.Message}");
            }
        }

        [HttpGet("total-benefits")]
        public async Task<ActionResult<int>> GetTotalBenefits()
        {
            try
            {
                // WILLIAM - Benefits API (port 5237)
                var response = await _httpClient.GetStringAsync("http://localhost:5237/api/benefits");
                var benefits = JsonSerializer.Deserialize<List<object>>(response);
                return benefits.Count;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fel vid hämtning av förmåner: {ex.Message}");
            }
        }

        [HttpGet("total-users")]
        public async Task<ActionResult<int>> GetTotalUsers()
        {
            try
            {
                // ANDREA - Users API (port 5094)
                var response = await _httpClient.GetStringAsync("http://localhost:5094/api/users");
                var users = JsonSerializer.Deserialize<List<object>>(response);
                return users.Count;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fel vid hämtning av användare: {ex.Message}");
            }
        }
        
        // EXTRA - Om du vill ha en sammanställning av allt
        [HttpGet("dashboard-summary")]
        public async Task<ActionResult<object>> GetDashboardSummary()
        {
            try
            {
                var benefitsTask = _httpClient.GetStringAsync("http://localhost:5237/api/benefits");
                var usersTask = _httpClient.GetStringAsync("http://localhost:5094/api/users");
                
                // Vänta på båda anropen samtidigt
                await Task.WhenAll(benefitsTask, usersTask);
                
                var benefits = JsonSerializer.Deserialize<List<object>>(await benefitsTask);
                var users = JsonSerializer.Deserialize<List<object>>(await usersTask);
                
                return Ok(new
                {
                    TotalBenefits = benefits.Count,
                    TotalUsers = users.Count,
                    // Applications tillkommer när du får Christoffers port
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fel: {ex.Message}");
            }
        }
    }
}