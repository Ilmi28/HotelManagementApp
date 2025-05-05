using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Cities.GetByCountry;

public class GetCitiesByCountryQueryHandler(ICityRepository cityRepository) : IRequestHandler<GetCitiesByCountryQuery, ICollection<CityResponse>>
{
    public async Task<ICollection<CityResponse>> Handle(GetCitiesByCountryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var cities = await cityRepository.GetCitiesByCountry(request.Country.Normalize(), cancellationToken);
        return cities.Select(c => new CityResponse
        {
            Id = c.Id,
            Name = c.Name,
            Country = c.Country,
            Latitude = c.Latitude,
            Longitude = c.Longitude
        }).ToList();
    }
}
