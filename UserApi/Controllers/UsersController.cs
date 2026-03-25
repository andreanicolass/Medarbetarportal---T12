using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using UserApi.Data;
using UserApi.Models;
using UserApi.DTOs;

namespace UserApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET USERS (ADMIN)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Department)
            .ToListAsync();

        return Ok(users.Select(u => new
        {
            u.Id,
            Name = u.FirstName + " " + u.LastName,
            u.Email,
            Role = u.Role!.Name,
            Department = u.Department!.Name,
            u.IsApproved
        }));
    }

    // GET ROLES
    [HttpGet("roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRoles()
    {
        return Ok(await _context.Roles.ToListAsync());
    }

    // GET DEPARTMENTS
    [HttpGet("departments")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDepartments()
    {
        return Ok(await _context.Departments.ToListAsync());
    }

    // UPDATE USER (ROLE + DEPARTMENT)
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        user.RoleId = dto.RoleId;
        user.DepartmentId = dto.DepartmentId;

        await _context.SaveChangesAsync();

        return Ok();
    }
}