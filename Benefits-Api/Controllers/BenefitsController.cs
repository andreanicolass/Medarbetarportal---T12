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

    // GET: api/Benefits (Hämtar alla förmåner)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Benefit>>> GetBenefits()
    {
        return await _context.Benefits.Include(b => b.Category).ToListAsync();
    }

    // POST: api/Benefits (Lägger till en ny förmån)
    [HttpPost]
    public async Task<ActionResult<Benefit>> PostBenefit(Benefit benefit)
    {
        _context.Benefits.Add(benefit);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBenefits), new { id = benefit.Id }, benefit);
    }
}