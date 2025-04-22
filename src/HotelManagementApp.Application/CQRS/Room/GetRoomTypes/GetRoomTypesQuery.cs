using HotelManagementApp.Application.Responses.RoomResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Room.GetRoomTypes;

public class GetRoomTypesQuery : IRequest<ICollection<RoomTypeResponse>>
{

}
