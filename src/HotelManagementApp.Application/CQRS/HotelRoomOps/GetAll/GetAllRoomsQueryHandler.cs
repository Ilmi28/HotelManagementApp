using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetAll;

public class GetAllRoomsQueryHandler(
    IRoomRepository roomRepository, 
    IRoomImageRepository imageRepository,
    IFileService fileService) : IRequestHandler<GetAllRoomsQuery, ICollection<RoomResponse>>
{
    public async Task<ICollection<RoomResponse>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllRooms(cancellationToken);
        var response = await Task.WhenAll(rooms.Select(async r => new RoomResponse
        {
            Id = r.Id,
            RoomName = r.RoomName,
            RoomType = r.RoomType.ToString(),
            Price = r.Price,
            HotelId = r.Hotel.Id,
            RoomImages = (await imageRepository.GetRoomImagesByRoomId(r.Id, cancellationToken))
            .Select(i => fileService.GetFileUrl("images", i.FileName)).ToList()
        }).ToList());
        return response;
    }
}
