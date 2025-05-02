using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelServiceOps.GetByHotel;

public class GetHotelServicesByHotelIdQuery : IRequest<ICollection<HotelServiceResponse>>
{
    public required int HotelId { get; set; }
}
