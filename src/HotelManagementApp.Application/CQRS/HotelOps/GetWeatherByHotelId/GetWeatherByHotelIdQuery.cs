using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelOps.GetWeatherByHotelId;

public class GetWeatherByHotelIdQuery : IRequest<WeatherResponse>
{
    public int HotelId { get; set; }
}