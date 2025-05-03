using HotelManagementApp.Application.Responses.RoomResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetRoomTypes;

public class GetRoomTypesQuery : IRequest<ICollection<RoomTypeResponse>>
{

}
