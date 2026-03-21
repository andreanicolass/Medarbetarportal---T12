using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationsApi.Models;
using ApplicationsApi.Data;

namespace ApplicationsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly AppDbContext _db;

    {
        _db = db;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var applications = await _db.Applications.ToListAsync();
        return Ok(applications);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var application = await _db.Applications.FindAsync(id);
        if (application == null)
            return NotFound();
        return Ok(application);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BenefitApplication application)
    {
        application.CreatedDate = DateTime.Now;
        application.Status = "Pending";
        _db.Applications.Add(application);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = application.Id }, application);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BenefitApplication application)
    {
        if (id != application.Id)
            return BadRequest();
        _db.Entry(application).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

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
    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<BenefitApplication>>> GetByEmployee(string employeeId)
    {
        return await _db.Applications
            .Where(a => a.EmployeeId == employeeId)
            .ToListAsync();
    }
}