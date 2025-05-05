using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelOps.Add;

public class AddHotelCommandHandler(IHotelRepository hotelRepository, ICityRepository cityRepository) : IRequestHandler<AddHotelCommand>
{
    public async Task Handle(AddHotelCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var city = await cityRepository.GetCityById(request.CityId, cancellationToken)
            ?? throw new CityNotFoundException($"City with id {request.CityId} doesn't exist");
        var hotelModel = new Hotel
        {
            Name = request.Name,
            Address = request.Address,
            City = city,
            Description = request.Description,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email
        };

        await hotelRepository.AddHotel(hotelModel, cancellationToken);
    }
}
