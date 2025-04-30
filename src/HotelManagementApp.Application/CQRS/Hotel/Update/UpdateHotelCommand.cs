using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.Hotel.Update;

public class UpdateHotelCommand : IRequest
{
    [Required]
    public int HotelId { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string Name { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string Address { get; set; }
    [Required]
    public required int CityId { get; set; }
    [Required]
    [Phone]
    public required string PhoneNumber { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    [MinLength(50)]
    [MaxLength(500)]
    public required string Description { get; set; }
}
