using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.OrderModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.AddReservation;

public class AddReservationCommandHandler(
    IOrderRepository orderRepository,
    IRoomRepository roomRepository,
    IReservationRepository reservationRepository) : IRequestHandler<AddReservationCommand>
{
    public async Task Handle(AddReservationCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderById(request.OrderId, cancellationToken)
            ?? throw new OrderNotFoundException($"Order with id {request.OrderId} not found");
        var room = await roomRepository.GetRoomById(request.RoomId, cancellationToken)
            ?? throw new RoomNotFoundException($"Room with id {request.RoomId} not found");
        var isRoomAvailable = await IsRoomAvailable(request.RoomId, request.From, request.To, cancellationToken);
        if (!isRoomAvailable)
            throw new ReservationConflictException($"Room with id {request.RoomId} is not available for the period of {request.From} - {request.To} ");
        if (order.Status is OrderStatusEnum.Cancelled or OrderStatusEnum.Confirmed)
            throw new InvalidOperationException($"Order with id {order.Id} is cancelled or confirmed. You can't make a reservation for it.");
        var reservation = new Reservation
        {
            Order = order,
            Room = room,
            From = request.From,
            To = request.To
        };
        await reservationRepository.AddReservation(reservation, cancellationToken);
    }

    private async Task<bool> IsRoomAvailable(int roomId, DateOnly from, DateOnly to, CancellationToken cancellationToken)
    {
        var result = true;
        var reservations = await reservationRepository.GetReservationsByRoomId(roomId, cancellationToken);
        foreach (var reservation in reservations)
        {
            if (reservation.From <= to && reservation.To >= from)
            {
                result = false;
                break;
            }
        }
        return result;
    }
}
