namespace UserApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using UserApi.Data;  
using UserApi.Models;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public UsersController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // GET: api/users
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromHeader(Name = "x-api-key")] string apiKey)
    {
        // 🔐 API-KEY CHECK (från config)
        if (apiKey != _config["ApiKey"])
        {
            return Unauthorized("Invalid API Key");
        }

        var users = await _context.Users
            .Include(u => u.Department) // 🔥 viktigt!
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.EmployeeId,
                u.DepartmentId,
                DepartmentName = u.Department != null ? u.Department.Name : null
            })
            .ToListAsync();

        return Ok(users);
    }

    // POST: api/users
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser(User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var departmentExists = await _context.Departments
            .AnyAsync(d => d.Id == user.DepartmentId);

        if (!departmentExists)
        {
            return BadRequest("Department does not exist");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, new
        {
            user.Id,
            user.Email
        });
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, User updatedUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != updatedUser.Id)
        {
            return BadRequest("Id mismatch");
        }

        var existingUser = await _context.Users.FindAsync(id);

        if (existingUser == null)
        {
            return NotFound();
        }

        var departmentExists = await _context.Departments
            .AnyAsync(d => d.Id == updatedUser.DepartmentId);

        if (!departmentExists)
        {
            return BadRequest("Department does not exist");
        }

        // Uppdatera
        existingUser.FirstName = updatedUser.FirstName;
        existingUser.LastName = updatedUser.LastName;
        existingUser.Email = updatedUser.Email;
        existingUser.EmployeeId = updatedUser.EmployeeId;
        existingUser.DepartmentId = updatedUser.DepartmentId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}