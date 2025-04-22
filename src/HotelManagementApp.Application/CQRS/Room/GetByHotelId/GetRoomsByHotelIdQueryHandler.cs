using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Room.GetByHotelId;

public class GetRoomsByHotelIdQueryHandler(IRoomRepository roomRepository)
    : IRequestHandler<GetRoomsByHotelIdQuery, ICollection<RoomResponse>>
{
    public async Task<ICollection<RoomResponse>> Handle(GetRoomsByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var roomModels = await roomRepository.GetRoomsByHotelId(request.HotelId, cancellationToken);
        return roomModels.Select(r => new RoomResponse
        {
            Id = r.Id,
            RoomName = r.RoomName,
            RoomType = r.RoomType.ToString(),
            Price = r.Price,
            HotelId = r.Hotel.Id,
        }).ToList();
    }
}
