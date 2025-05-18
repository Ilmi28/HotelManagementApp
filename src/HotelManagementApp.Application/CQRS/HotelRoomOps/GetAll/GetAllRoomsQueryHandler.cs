using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetAll;

public class GetAllRoomsQueryHandler(
    IRoomRepository roomRepository, 
    IRoomImageRepository imageRepository,
    IFileService fileService,
    IPricingService pricingService) : IRequestHandler<GetAllRoomsQuery, ICollection<RoomResponse>>
{
    public async Task<ICollection<RoomResponse>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllRooms(cancellationToken);
        var response = new List<RoomResponse>();
        foreach (var room in rooms)
        {
            var finalPrice = await pricingService.CalculatePriceForRoom(room, cancellationToken);
            response.Add(new RoomResponse
            {
                Id = room.Id,
                RoomName = room.RoomName,
                RoomType = room.RoomType.ToString(),
                Price = room.Price,
                HotelId = room.Hotel.Id,
                RoomImages = (await imageRepository.GetRoomImagesByRoomId(room.Id, cancellationToken))
                    .Select(i => fileService.GetFileUrl("images", i.FileName)).ToList(),
                DiscountPercent = 100 - (100 * finalPrice / room.Price),
                FinalPrice = finalPrice,
            });
        }
        return response;
    }
}
