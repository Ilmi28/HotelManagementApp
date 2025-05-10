using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.GetReservationParkings;

public class GetReservationParkingsQueryHandler(
    IReservationRepository reservationRepository,
    IReservationParkingRepository reservationParkingRepository) : IRequestHandler<GetReservationParkingsQuery, ICollection<ReservationParkingResponse>>
{
    public async Task<ICollection<ReservationParkingResponse>> Handle(GetReservationParkingsQuery request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetReservationById(request.ReservationId, cancellationToken)
            ?? throw new ReservationNotFoundException($"Reservation with id {request.ReservationId} not found");
        var reservationParkings = await reservationParkingRepository.GetReservationParkingsByReservationId(request.ReservationId, cancellationToken);
        var response = new List<ReservationParkingResponse>();
        foreach (var reservationParking in reservationParkings)
        {
            response.Add(new ReservationParkingResponse
            {
                ReservationId = reservationParking.Reservation.Id,
                ParkingId = reservationParking.HotelParking.Id,
            });
        }
        return response;
    }
}