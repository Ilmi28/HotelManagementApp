using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.Account.ChangePassword;

public class ChangePasswordCommand : IRequest
{
    public required string UserId { get; set; }
    public required string OldPassword { get; set; }
    [Required(ErrorMessage = "Password field is required.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters long, include an uppercase letter, a lowercase letter, a number, and a special character.")]
    public required string NewPassword { get; set; }
}
