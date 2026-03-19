using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationsApi.Models;
using ApplicationsApi.Data;
using ApplicationsApi.DTOs;
using System.Net.Http.Json;



namespace ApplicationsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;

    public ApplicationsController(AppDbContext db, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
    }
    
    // GET: api/applications
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var applications = await _db.Applications.ToListAsync();
        return Ok(applications);
    }

    // GET: api/applications/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var application = await _db.Applications.FindAsync(id);
        if (application == null)
            return NotFound();
        return Ok(application);
    }

    // GET: api/applications/{id}/details
    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(int id)
    {
        var application = await _db.Applications.FindAsync(id);
        if (application == null)
            return NotFound();

        var userClient = _httpClientFactory.CreateClient("UserApi");
        var benefitClient = _httpClientFactory.CreateClient("BenefitApi");

        var user = await userClient.GetFromJsonAsync<UserDto>($"api/users/employee/{application.EmployeeId}");
        var benefit = await benefitClient.GetFromJsonAsync<BenefitDto>($"api/benefits/{application.BenefitId}");

        return Ok(new
        {
            application.Id,
            application.Status,
            application.CreatedDate,
            application.DecisionDate,
            User = user,
            Benefit = benefit
        });
    }
    
    // POST: api/applications
    [HttpPost]
    public async Task<IActionResult> Create(BenefitApplication application)
    {
        application.CreatedDate = DateTime.Now;
        application.Status = "Pending";
        
        _db.Applications.Add(application);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = application.Id }, application);
    }

    // PUT: api/applications/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BenefitApplication application)
    {
        if (id != application.Id)
            return BadRequest();
        
        var exists = await _db.Applications.AnyAsync(a => a.Id == id);
        if (!exists)
            return NotFound();
        
        _db.Entry(application).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/applications/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var application = await _db.Applications.FindAsync(id);
        if (application == null)
            return NotFound();
        
        _db.Applications.Remove(application);
        await _db.SaveChangesAsync();
        return NoContent();
    }
    // GET: api/applications/employee/{employeeId}
    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<BenefitApplication>>> GetByEmployee(string employeeId)
    {
        return await _db.Applications
            .Where(a => a.EmployeeId == employeeId)
            .ToListAsync();
    }
}