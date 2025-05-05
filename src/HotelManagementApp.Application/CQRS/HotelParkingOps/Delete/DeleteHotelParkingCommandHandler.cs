using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelParkingOps.Delete;

public class DeleteHotelParkingCommandHandler(
    IHotelParkingRepository parkingRepository) : IRequestHandler<DeleteHotelParkingCommand>
{
    public async Task Handle(DeleteHotelParkingCommand request, CancellationToken cancellationToken)
    {
        var parking = await parkingRepository.GetHotelParkingById(request.Id, cancellationToken)
            ?? throw new HotelParkingNotFoundException($"Hotel parking with id {request.Id} not found");
        await parkingRepository.DeleteHotelParking(parking, cancellationToken);
    }
}
