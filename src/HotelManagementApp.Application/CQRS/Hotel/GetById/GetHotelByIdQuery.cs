using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Hotel.GetById;

public class GetHotelByIdQuery : IRequest<HotelResponse>
{
    public int HotelId { get; set; }
}
