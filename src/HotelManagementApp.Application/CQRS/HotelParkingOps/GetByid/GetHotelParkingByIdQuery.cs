using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelParkingOps.GetByid;

public class GetHotelParkingByIdQuery : IRequest<HotelParkingResponse>
{
    public required int ParkingId { get; set; }
}