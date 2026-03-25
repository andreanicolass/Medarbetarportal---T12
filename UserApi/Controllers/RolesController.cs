using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using UserApi.Data;
using UserApi.Models;

namespace UserApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/roles
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _context.Roles.ToListAsync();
        return Ok(roles);
    }

    // POST: api/roles
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRole(Role role)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return Ok(role);
    }

    // DELETE: api/roles/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null)
        {
            return NotFound();
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}