using HotelManagementApp.Application.Responses.RoomResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetById;

public class GetRoomByIdQuery : IRequest<RoomResponse>
{
    public int RoomId { get; set; }
}
