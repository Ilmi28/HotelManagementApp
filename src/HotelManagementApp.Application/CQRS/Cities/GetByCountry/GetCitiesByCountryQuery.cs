using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Cities.GetByCountry;

public class GetCitiesByCountryQuery : IRequest<ICollection<CityResponse>>
{
    public required string Country { get; set; }
}
