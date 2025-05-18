using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetByHotelId;

public class GetRoomsByHotelIdQueryHandler(
    IRoomRepository roomRepository, 
    IHotelRepository hotelRepository,
    IRoomImageRepository imageRepository,
    IFileService fileService,
    IPricingService pricingService)
    : IRequestHandler<GetRoomsByHotelIdQuery, ICollection<RoomResponse>>
{
    public async Task<ICollection<RoomResponse>> Handle(GetRoomsByHotelIdQuery request, CancellationToken cancellationToken)
    {
        _ = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var roomModels = await roomRepository.GetRoomsByHotelId(request.HotelId, cancellationToken);
        var response = new List<RoomResponse>();
        foreach (var room in roomModels)
        {
            var finalPrice = await pricingService.CalculatePriceForRoom(room, cancellationToken);
            var roomResponse = new RoomResponse
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
            };
            response.Add(roomResponse);
        }
        return response;
    }
}
