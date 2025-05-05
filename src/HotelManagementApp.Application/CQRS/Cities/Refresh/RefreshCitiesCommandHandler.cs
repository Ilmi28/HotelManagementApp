using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Cities.Refresh;

public class RefreshCitiesCommandHandler(ICityRepository cityRepository, ICityService cityService) : IRequestHandler<RefreshCitiesCommand>
{
    public async Task Handle(RefreshCitiesCommand request, CancellationToken cancellationToken)
    {
        await foreach (var city in cityService.FetchCities(cancellationToken))
        {
            ArgumentNullException.ThrowIfNull(city, nameof(request));
            var existingCity = await cityRepository.GetCityById(city.Id, cancellationToken);
            if (existingCity == null)
            {
                await cityRepository.AddCity(city, cancellationToken);
            }
            else
            {
                existingCity.Name = city.Name;
                existingCity.Latitude = city.Latitude;
                existingCity.Longitude = city.Longitude;
                existingCity.Country = city.Country;
                await cityRepository.UpdateCity(existingCity, cancellationToken);
            }
        }
    }
}
