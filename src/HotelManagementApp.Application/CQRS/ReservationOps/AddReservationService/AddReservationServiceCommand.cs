using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.AddReservationService;

public class AddReservationServiceCommand : IRequest
{
    public required int ReservationId { get; set; }
    public required int ServiceId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public required int Quantity { get; set; }
}