using HotelManagementApp.Application.Responses.OrderResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.GetReservationServices;

public class GetReservationServicesQuery : IRequest<ICollection<ReservationServiceResponse>>
{
    public required int ReservationId { get; set; }
}