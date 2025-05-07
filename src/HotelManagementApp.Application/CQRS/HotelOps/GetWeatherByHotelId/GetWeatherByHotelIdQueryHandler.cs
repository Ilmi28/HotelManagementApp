using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelOps.GetWeatherByHotelId;

public class GetWeatherByHotelIdQueryHandler(
    IHotelRepository hotelRepository, 
    IWeatherService weatherService) : IRequestHandler<GetWeatherByHotelIdQuery, WeatherResponse>
{
    public async Task<WeatherResponse> Handle(GetWeatherByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var temperature = await weatherService.GetTemperatureAsync(hotel.City.Latitude, hotel.City.Longitude);
        return new WeatherResponse
        {
            Temperature = temperature
        };
    }
}