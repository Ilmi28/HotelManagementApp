using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.HotelServiceOps.Add;

public class AddHotelServiceCommand : IRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string Name { get; set; }
    [Required]
    [MinLength(10)]
    [MaxLength(100)]
    public required string Description { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public required decimal Price { get; set; }
    [Required]
    public required int HotelId { get; set; }
}
