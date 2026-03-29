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
                var response = await _httpClient.GetStringAsync(
                    "https://medarbetarportal-applicationsapi-ewfceye9cncmcnb3.polandcentral-01.azurewebsites.net/api/applications"
                );

                var applications = JsonSerializer.Deserialize<List<object>>(response);
                return applications?.Count ?? 0;
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
                // ⚠️ ÄNDRA till riktig Azure URL om ni har den (William)
                var response = await _httpClient.GetStringAsync(
                    "https://medarbetarportal-benefits-william-hcb4ehh6cnd2d8f8.polandcentral-01.azurewebsites.net/api/benefits "
                );

                var benefits = JsonSerializer.Deserialize<List<object>>(response);
                return benefits?.Count ?? 0;
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
                var response = await _httpClient.GetStringAsync(
                    "https://medarbetarportal-userapi.azurewebsites.net/api/users"
                );

                var users = JsonSerializer.Deserialize<List<object>>(response);
                return users?.Count ?? 0;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fel vid hämtning av användare: {ex.Message}");
            }
        }
        
        [HttpGet("dashboard-summary")]
        public async Task<ActionResult<object>> GetDashboardSummary()
        {
            try
            {
                var benefitsTask = _httpClient.GetStringAsync(
                    "https://medarbetarportal-economyapi.azurewebsites.net/api/benefits"
                );

                var usersTask = _httpClient.GetStringAsync(
                    "https://medarbetarportal-userapi.azurewebsites.net/api/users"
                );

                await Task.WhenAll(benefitsTask, usersTask);

                var benefits = JsonSerializer.Deserialize<List<object>>(await benefitsTask);
                var users = JsonSerializer.Deserialize<List<object>>(await usersTask);

                return Ok(new
                {
                    TotalBenefits = benefits?.Count ?? 0,
                    TotalUsers = users?.Count ?? 0,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fel: {ex.Message}");
            }
        }
    }
}