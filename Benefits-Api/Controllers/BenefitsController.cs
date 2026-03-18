using Benefits_Api.Data;
using Benefits_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Benefits_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BenefitsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BenefitsController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Benefit>>> GetBenefits()
    {
        return await _context.Benefits.Include(b => b.Category).ToListAsync();
    }
    
    [HttpPost]
    public async Task<ActionResult<Benefit>> PostBenefit(Benefit benefit)
    {
        _context.Benefits.Add(benefit);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBenefits), new { id = benefit.Id }, benefit);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBenefit(int id, Benefit benefit)
    {
        if (id != benefit.Id)
        {
            return BadRequest("ID i URL:en stämmer inte överens med objektets ID.");
        }

        _context.Entry(benefit).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BenefitExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBenefit(int id)
    {
        var benefit = await _context.Benefits.FindAsync(id);
        if (benefit == null)
        {
            return NotFound();
        }

        _context.Benefits.Remove(benefit);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    private bool BenefitExists(int id)
    {
        return _context.Benefits.Any(e => e.Id == id);
    }
}