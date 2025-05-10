using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Identity;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.ReservationRepositores;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetOrderReservations;

public class GetOrderReservationsQueryHandler(
    IOrderRepository orderRepository,
    IReservationRepository reservationRepository) : IRequestHandler<GetOrderReservationsQuery, ICollection<ReservationResponse>>
{
    public async Task<ICollection<ReservationResponse>> Handle(GetOrderReservationsQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderById(request.OrderId, cancellationToken)
            ?? throw new OrderNotFoundException($"Order with id {request.OrderId} not found");
        var reservations = await reservationRepository.GetReservationsByOrderId(request.OrderId, cancellationToken);
        var response = new List<ReservationResponse>();
        foreach (var reservation in reservations)
        {
            response.Add(new ReservationResponse
            {
                Id = reservation.Id,
                RoomId = reservation.Room.Id,
                From = reservation.From,
                To = reservation.To,
                UserId = reservation.Order.UserId,
                OrderId = reservation.Order.Id,
            });
        }
        return response;
    }

    
}