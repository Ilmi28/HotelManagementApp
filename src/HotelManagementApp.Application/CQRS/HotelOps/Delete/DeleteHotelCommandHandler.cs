using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelOps.Delete;

public class DeleteHotelCommandHandler(IHotelRepository hotelRepository) : IRequestHandler<DeleteHotelCommand>
{
    public async Task Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var hotelModel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        await hotelRepository.RemoveHotel(request.HotelId, cancellationToken);
    }
}
