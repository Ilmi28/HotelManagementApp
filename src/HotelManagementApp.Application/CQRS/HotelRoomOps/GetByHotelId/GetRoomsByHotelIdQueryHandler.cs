using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetByHotelId;

public class GetRoomsByHotelIdQueryHandler(
    IRoomRepository roomRepository, 
    IRoomImageRepository imageRepository,
    IFileService fileService,
    IHotelDiscountRepository hotelDiscountRepository,
    IRoomDiscountRepository roomDiscountRepository)
    : IRequestHandler<GetRoomsByHotelIdQuery, ICollection<RoomResponse>>
{
    public async Task<ICollection<RoomResponse>> Handle(GetRoomsByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var roomModels = await roomRepository.GetRoomsByHotelId(request.HotelId, cancellationToken);
        var hotelDiscounts = await hotelDiscountRepository.GetDiscountsByTypeId(request.HotelId, cancellationToken);
        var totalDiscountPercent = 0;
        int totalHotelDiscountPercent = 0;
        foreach (var discount in hotelDiscounts)
        {
            if (discount.From < DateTime.Now && discount.To > DateTime.Now)
                totalHotelDiscountPercent += discount.DiscountPercent;
        }
        var response = new List<RoomResponse>();
        foreach (var room in roomModels)
        {
            totalDiscountPercent = totalHotelDiscountPercent;
            var roomDiscounts = await roomDiscountRepository.GetDiscountsByTypeId(room.Id, cancellationToken);
            foreach (var discount in roomDiscounts)
            {
                if (discount.From < DateTime.Now && discount.To > DateTime.Now)
                    totalDiscountPercent += discount.DiscountPercent;
            }
            var roomResponse = new RoomResponse
            {
                Id = room.Id,
                RoomName = room.RoomName,
                RoomType = room.RoomType.ToString(),
                Price = room.Price - (room.Price * totalDiscountPercent / 100),
                HotelId = room.Hotel.Id,
                RoomImages = (await imageRepository.GetRoomImagesByRoomId(room.Id, cancellationToken))
                    .Select(i => fileService.GetFileUrl("images", i.FileName)).ToList(),
                DiscountPercent = totalDiscountPercent <= 100 ? totalDiscountPercent : 100,
            };
            response.Add(roomResponse);
        }
        return response;
    }
}
