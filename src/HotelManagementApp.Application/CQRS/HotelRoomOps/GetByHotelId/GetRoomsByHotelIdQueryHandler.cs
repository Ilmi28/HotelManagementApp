using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetByHotelId;

public class GetRoomsByHotelIdQueryHandler(
    IRoomRepository roomRepository, 
    IRoomImageRepository imageRepository,
    IFileService fileService)
    : IRequestHandler<GetRoomsByHotelIdQuery, ICollection<RoomResponse>>
{
    public async Task<ICollection<RoomResponse>> Handle(GetRoomsByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var roomModels = await roomRepository.GetRoomsByHotelId(request.HotelId, cancellationToken);
        var response = await Task.WhenAll(roomModels.Select(async r => new RoomResponse
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
