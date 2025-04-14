using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.Account.Create;

public class CreateAccountCommand : IRequest
{
    [Required(ErrorMessage = "Username field is required")]
    [MinLength(3)]
    [MaxLength(50)]
    public required string UserName { get; set; }
    [Required(ErrorMessage = "Email field is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }
    [Required(ErrorMessage = "Password field is required.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters long, include an uppercase letter, a lowercase letter, a number, and a special character.")]
    public required string Password { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}
