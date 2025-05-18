using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.CQRS.HotelParkingOps.Add;

public class AddHotelParkingCommand : IRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int CarSpaces { get; set; }
    [MaxLength(100)]
    public string? Description { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public required int Price { get; set; }
    public int HotelId { get; set; }
}
