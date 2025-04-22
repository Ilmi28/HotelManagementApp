using HotelManagementApp.Application.Responses.RoomResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Room.GetAll;

public class GetAllRoomsQuery : IRequest<ICollection<RoomResponse>>
{

}
