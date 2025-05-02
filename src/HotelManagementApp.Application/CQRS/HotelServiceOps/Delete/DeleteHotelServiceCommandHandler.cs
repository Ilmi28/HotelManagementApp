using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelServiceOps.Delete;

public class DeleteHotelServiceCommandHandler(IHotelServiceRepository hotelServiceRepository) : IRequestHandler<DeleteHotelServiceCommand>
{
    public async Task Handle(DeleteHotelServiceCommand request, CancellationToken cancellationToken)
    {
        var hotelService = await hotelServiceRepository.GetHotelServiceById(request.HotelServiceId, cancellationToken)
            ?? throw new HotelServiceNotFoundException($"Hotel service with id {request.HotelServiceId} not found");
        await hotelServiceRepository.DeleteHotelService(hotelService, cancellationToken);
    }
}
