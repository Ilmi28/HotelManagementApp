using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.Account.Update;

public class UpdateAccountCommand : IRequest
{
    public required string UserId { get; set; }
    [MinLength(3)]
    [MaxLength(50)]
    [Required]
    public required string UserName { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
