using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.API.Requests;

public class CreateAccountRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string UserName { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters long, include an uppercase letter, a lowercase letter, a number, and a special character.")]
    public required string Password { get; set; }
}
