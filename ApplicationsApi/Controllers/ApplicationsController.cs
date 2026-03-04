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

    public ApplicationsController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/applications
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var applications = await _db.Applications.ToListAsync();
        return Ok(applications);
    }

    // GET api/applications/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var application = await _db.Applications.FindAsync(id);
        if (application == null)
            return NotFound();
        return Ok(application);
    }

    // POST api/applications
    [HttpPost]
    public async Task<IActionResult> Create(BenefitApplication application)
    {
        application.CreatedDate = DateTime.Now;
        application.Status = "Pending";
        _db.Applications.Add(application);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = application.Id }, application);
    }

    // PUT api/applications/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BenefitApplication application)
    {
        if (id != application.Id)
            return BadRequest();
        _db.Entry(application).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/applications/5
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
    // GET: api/applications/employee/5
    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<BenefitApplication>>> GetByEmployee(string employeeId)
    {
        return await _db.Applications
            .Where(a => a.EmployeeId == employeeId)
            .ToListAsync();
    }
}