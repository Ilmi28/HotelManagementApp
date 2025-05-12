using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetAll;

public class GetAllRoomsQueryHandler(
    IRoomRepository roomRepository, 
    IRoomImageRepository imageRepository,
    IFileService fileService,
    IRoomDiscountService discountService) : IRequestHandler<GetAllRoomsQuery, ICollection<RoomResponse>>
{
    public async Task<ICollection<RoomResponse>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllRooms(cancellationToken);
        var response = new List<RoomResponse>();
        foreach (var room in rooms)
        {
            var discount = await discountService.CalculateDiscount(room, cancellationToken);
            response.Add(new RoomResponse
            {
                Id = room.Id,
                RoomName = room.RoomName,
                RoomType = room.RoomType.ToString(),
                Price = room.Price,
                HotelId = room.Hotel.Id,
                RoomImages = (await imageRepository.GetRoomImagesByRoomId(room.Id, cancellationToken))
                    .Select(i => fileService.GetFileUrl("images", i.FileName)).ToList(),
                DiscountPercent = discount,
                FinalPrice = room.Price - (room.Price * discount / 100),
            });
        }
        return response;
    }
}
