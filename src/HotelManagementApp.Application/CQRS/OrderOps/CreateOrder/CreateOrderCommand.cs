using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.CreateOrder;

public class CreateOrderCommand : IRequest<int>
{
    [Required]
    public required string UserId { get; set; }
    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }
    [Required]
    [MaxLength(50)]   
    public required string LastName { get; set; }
    [Required]
    [Phone]  
    public required string PhoneNumber { get; set; }
    [Required] 
    [MaxLength(255)]
    public required string Address { get; set; }
    [Required]
    [MaxLength(50)]
    public required string City { get; set; }
    [Required]
    [MaxLength(50)]
    public required string Country { get; set; }
}
