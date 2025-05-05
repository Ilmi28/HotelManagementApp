using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelParkingOps.Update;

public class UpdateHotelParkingCommandHandler(
    IHotelRepository hotelRepository,
    IHotelParkingRepository parkingRepository) : IRequestHandler<UpdateHotelParkingCommand>
{
    public async Task Handle(UpdateHotelParkingCommand request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var parking = await parkingRepository.GetHotelParkingById(request.Id, cancellationToken)
            ?? throw new HotelParkingNotFoundException($"Hotel parking with id {request.Id} not found");
        parking.CarSpaces = request.CarSpaces;
        parking.Description = request.Description;
        parking.Price = request.Price;
        parking.Hotel = hotel;
        await parkingRepository.UpdateHotelParking(parking, cancellationToken);
    }
}
