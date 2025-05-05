using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.AddReservation;

public class AddReservationCommandHandler(
    IOrderRepository orderRepository,
    IPendingOrderRepository pendingOrderRepository) : IRequestHandler<AddReservationCommand>
{
    public async Task Handle(AddReservationCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
