using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.Conflict;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Models.OrderModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.ReservationOps.AddReservation;

public class AddReservationCommandHandler(
    IOrderRepository orderRepository,
    IRoomRepository roomRepository,
    IReservationRepository reservationRepository) : IRequestHandler<AddReservationCommand, int>
{
    public async Task<int> Handle(AddReservationCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetRoomById(request.RoomId, cancellationToken)
                   ?? throw new RoomNotFoundException($"Room with id {request.RoomId} not found");
        if (request.From < DateOnly.FromDateTime(DateTime.Today) || request.From > request.To)
            throw new InvalidOperationException("Invalid date chosen, date should be in present and to date should sooner than from date");
        var order = await orderRepository.GetOrderById(request.OrderId, cancellationToken)
            ?? throw new OrderNotFoundException($"Order with id {request.OrderId} not found");
        if (order.Status is OrderStatusEnum.Completed or OrderStatusEnum.Cancelled or OrderStatusEnum.Confirmed)
            throw new InvalidOperationException($"Order with id {request.OrderId} has invalid status. Status: {order.Status}");
        var isRoomAvailable = await IsRoomAvailable(request.RoomId, request.From, request.To, cancellationToken);
        if (!isRoomAvailable)
            throw new ReservationConflictException($"Room with id {request.RoomId} is not available for the period of {request.From} - {request.To} ");
        if (order.Status is OrderStatusEnum.Cancelled or OrderStatusEnum.Confirmed)
            throw new InvalidOperationException($"Order with id {order.Id} is cancelled or confirmed. You can't modify a reservation for it.");
        var reservation = new Reservation
        {
            Order = order,
            Room = room,
            From = request.From,
            To = request.To
        };
        await reservationRepository.AddReservation(reservation, cancellationToken);
        return reservation.Id;
    }

    private async Task<bool> IsRoomAvailable(int roomId, DateOnly from, DateOnly to, CancellationToken cancellationToken)
    {
        var result = true;
        var reservations = await reservationRepository.GetReservationsByRoomId(roomId, cancellationToken);
        foreach (var reservation in reservations)
        {
            if (reservation.Order.Status is OrderStatusEnum.Cancelled or OrderStatusEnum.Pending 
                || reservation.From <= to && reservation.To >= from)
            {
                result = false;
                break;
            }
        }
        return result;
    }
}
