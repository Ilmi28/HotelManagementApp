using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Cities.GetById;

public class GetCityByIdQueryHandler(ICityRepository cityRepository) : IRequestHandler<GetCityByIdQuery, CityResponse>
{
    public async Task<CityResponse> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var city = await cityRepository.GetCityById(request.Id, cancellationToken)
            ?? throw new CityNotFoundException($"City with id {request.Id} not found");
        return new CityResponse
        {
            Id = city.Id,
            Name = city.Name,
            Country = city.Country,
            Latitude = city.Latitude,
            Longitude = city.Longitude
        };
    }
}
