using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.HotelServiceOps.Update;

public class UpdateHotelServiceCommand : IRequest
{
    public required int Id { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string Name { get; set; }
    [Required]
    [MinLength(10)]
    [MaxLength(200)]
    public required string Description { get; set; }
    [Required]
    public required decimal Price { get; set; }
    [Required]
    public required int HotelId { get; set; }
}
