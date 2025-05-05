using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelOps.Update;

public class UpdateHotelCommandHandler(IHotelRepository hotelRepository, ICityRepository cityRepository) : IRequestHandler<UpdateHotelCommand>
{
    public async Task Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var city = await cityRepository.GetCityById(request.CityId, cancellationToken)
            ?? throw new CityNotFoundException($"City with id {request.CityId} doesn't exist");
        var hotelModel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        hotelModel.Name = request.Name;
        hotelModel.Address = request.Address;
        hotelModel.City = city;
        hotelModel.Description = request.Description;
        hotelModel.PhoneNumber = request.PhoneNumber;
        hotelModel.Email = request.Email;
        await hotelRepository.UpdateHotel(hotelModel, cancellationToken);
    }
}
