using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetById;

public class GetRoomByIdQueryHandler(
    IRoomRepository roomRepository, 
    IRoomImageRepository imageRepository,
    IFileService fileService) : IRequestHandler<GetRoomByIdQuery, RoomResponse>
{
    public async Task<RoomResponse> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var roomModel = await roomRepository.GetRoomById(request.RoomId, cancellationToken)
            ?? throw new RoomNotFoundException($"Room with id {request.RoomId} not found");
        return new RoomResponse
        {
            RoomName = roomModel.RoomName,
            RoomType = roomModel.RoomType.ToString(),
            Price = roomModel.Price,
            HotelId = roomModel.Hotel.Id,
            RoomImages = (await imageRepository.GetRoomImagesByRoomId(roomModel.Id, cancellationToken))
                .Select(i => fileService.GetFileUrl("images", i.FileName)).ToList()
        };

    }
}
