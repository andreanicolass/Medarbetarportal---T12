using System.ComponentModel.DataAnnotations;
namespace UserApi.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Förnamn är obligatoriskt")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Efternamn är obligatoriskt")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Anställnings-ID är obligatoriskt")]
    public string EmployeeId { get; set; }

    [Required(ErrorMessage = "Email är obligatoriskt")]
    [EmailAddress(ErrorMessage = "Ogiltig email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Lösenord är obligatoriskt")]
    [MinLength(6, ErrorMessage = "Lösenordet måste vara minst 6 tecken")]
    public string Password { get; set; }
}