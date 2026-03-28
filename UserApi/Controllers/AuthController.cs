using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using UserApi.DTOs;
using UserApi.Data;
using UserApi.Models;

namespace UserApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    // LOGIN Temporär
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto? login)
    {
        try
        {
            if (login == null)
                return BadRequest("Login är null");

            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Email == login.Email);

            if (user == null)
                return Unauthorized("User finns inte");

            if (user.Password == null)
                return StatusCode(500, "Password är null i DB");

            bool isValid = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

            if (!isValid)
                return Unauthorized("Fel lösenord");

            var roleName = user.Role?.Name ?? "NO ROLE";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email ?? "NO EMAIL"),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("UserId", user.Id.ToString())
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims)));

            return Ok(new
            {
                user.Email,
                Role = roleName,
                Department = user.Department?.Name ?? "NO DEPT"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"ERROR: {ex.Message}");
        }
    }

    // REGISTER
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Password.Length < 6)
                return BadRequest("Lösenordet måste vara minst 6 tecken");

            if (!dto.Password.Any(char.IsUpper))
                return BadRequest("Måste innehålla en stor bokstav");

            if (!dto.Password.Any(char.IsDigit))
                return BadRequest("Måste innehålla en siffra");

            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return BadRequest("Email finns redan");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmployeeId = dto.EmployeeId,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsApproved = true,
                RoleId = 2,
                DepartmentId = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Registrerad! Vänta på godkännande.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"REGISTER ERROR: {ex.Message}");
        }
    }

    // LOGOUT
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok("Utloggad");
    }

    // ADMIN – GODKÄNN USER
    [HttpPut("approve/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.IsApproved = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "User godkänd" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"APPROVE ERROR: {ex.Message}");
        }
    }
}