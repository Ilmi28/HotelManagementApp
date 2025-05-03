using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelOps.GetById;

public class GetHotelByIdQuery : IRequest<HotelResponse>
{
    public int HotelId { get; set; }
}
