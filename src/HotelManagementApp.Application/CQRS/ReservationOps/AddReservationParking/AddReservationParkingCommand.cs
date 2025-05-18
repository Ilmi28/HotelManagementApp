using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.AddReservationParking;

public class AddReservationParkingCommand : IRequest
{
    public required int ReservationId { get; set; }
    public required int ParkingId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public required int Quantity { get; set; }
}