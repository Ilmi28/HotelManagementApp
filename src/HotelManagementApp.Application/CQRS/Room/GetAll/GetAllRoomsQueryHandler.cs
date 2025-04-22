using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Room.GetAll;

public class GetAllRoomsQueryHandler(IRoomRepository roomRepository) : IRequestHandler<GetAllRoomsQuery, ICollection<RoomResponse>>
{
    public async Task<ICollection<RoomResponse>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllRooms(cancellationToken);
        return rooms.Select(r => new RoomResponse
        {
            Id = r.Id,
            RoomName = r.RoomName,
            RoomType = r.RoomType.ToString(),
            Price = r.Price,
            HotelId = r.Hotel.Id
        }).ToList();
    }
}
