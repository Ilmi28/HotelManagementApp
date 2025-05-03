using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelParkingOps.GetByHotelId;

public class GetHotelParkingsByHotelIdQuery : IRequest<ICollection<HotelParkingResponse>>
{
    public required int HotelId { get; set; }
}
