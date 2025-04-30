using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Cities.GetById;

public class GetCityByIdQuery : IRequest<CityResponse>
{
    public required int Id { get; set; }
}
