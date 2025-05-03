using HotelManagementApp.Application.Responses.RoomResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetByHotelId;

public class GetRoomsByHotelIdQuery : IRequest<ICollection<RoomResponse>>
{
    public int HotelId { get; set; }
}
