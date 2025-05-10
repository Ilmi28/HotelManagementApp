using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.GetAvailableDays;

public class GetAvailableDaysQueryHandler(
    IReservationRepository reservationRepository, 
    IRoomRepository roomRepository) : IRequestHandler<GetAvailableDaysQuery, ICollection<DateOnly>>
{
    public async Task<ICollection<DateOnly>> Handle(GetAvailableDaysQuery request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetRoomById(request.RoomId, cancellationToken)
            ?? throw new RoomNotFoundException($"Room with id {request.RoomId} not found");
        var reservations = await reservationRepository.GetReservationsByRoomId(request.RoomId, cancellationToken);
        var allDays = new List<DateOnly>();
        var date = request.From;
        while (date <= request.To)
        {
            allDays.Add(date);
            date = date.AddDays(1);
        }
        var bookedDays = new List<DateOnly>();
        foreach (var reservation in reservations)
        {
            if (reservation.Order.Status is OrderStatusEnum.Cancelled or OrderStatusEnum.Pending)
                continue;
            date = reservation.From;
            while (date <= reservation.To)
            {
                bookedDays.Add(date);
                date = date.AddDays(1);
            }
        }
        return allDays.Except(bookedDays).ToList();
    }
}