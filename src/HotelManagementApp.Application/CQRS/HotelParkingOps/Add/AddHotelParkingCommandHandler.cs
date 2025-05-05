using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelParkingOps.Add;

public class AddHotelParkingCommandHandler(
    IHotelParkingRepository parkingRepository,
    IHotelRepository hotelRepository) : IRequestHandler<AddHotelParkingCommand>
{
    public async Task Handle(AddHotelParkingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var hotelParking = new HotelParking
        {
            CarSpaces = request.CarSpaces,
            Description = request.Description,
            Hotel = hotel,
            Price = request.Price,
        };
        await parkingRepository.AddHotelParking(hotelParking, cancellationToken);
    }
}
