using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.GetReservationServices;

public class GetReservationServicesQueryCommand(
    IReservationRepository reservationRepository,
    IReservationServiceRepository reservationServiceRepository) : IRequestHandler<GetReservationServicesQuery, ICollection<ReservationServiceResponse>>
{
    public async Task<ICollection<ReservationServiceResponse>> Handle(GetReservationServicesQuery request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetReservationById(request.ReservationId, cancellationToken)
            ?? throw new ReservationNotFoundException($"Reservation with id {request.ReservationId} not found");
        var reservationServices = await reservationServiceRepository.GetReservationServicesByReservationId(request.ReservationId, cancellationToken);
        var response = new List<ReservationServiceResponse>();
        foreach (var reservationService in reservationServices)
        {
            response.Add(new ReservationServiceResponse
            {
                ReservationId = reservationService.Reservation.Id,
                ServiceId = reservationService.HotelService.Id,
            });
        }
        return response;
    }
}