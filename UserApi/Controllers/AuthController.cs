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

    // LOGIN
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Email == login.Email);

        if (user == null)
            return Unauthorized("Fel email eller lösenord");
        
        bool isValid = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

        if (!isValid)
            return Unauthorized("Fel email eller lösenord");
        
        if (!user.IsApproved)
            return Unauthorized("Kontot är inte godkänt ännu");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role!.Name),
            new Claim("UserId", user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        //  RETURNERA USER 
        return Ok(new
        {
            user.Id,
            Name= user.FirstName + " " + user.LastName,
            user.Email,
            Role = user.Role?.Name,
            Department = user.Department?.Name
        });
    }

    // REGISTER
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
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
            IsApproved = false,
            RoleId = 2,
            DepartmentId = 1
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Registrerad! Vänta på godkännande.");
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
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        user.IsApproved = true;
        await _context.SaveChangesAsync();

        return Ok(new { message= "User godkänd"});
    }
    
}