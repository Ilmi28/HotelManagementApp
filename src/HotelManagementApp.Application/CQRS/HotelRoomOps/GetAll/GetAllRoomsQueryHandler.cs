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
    IHotelDiscountRepository hotelDiscountRepository,
    IRoomDiscountRepository roomDiscountRepository) : IRequestHandler<GetAllRoomsQuery, ICollection<RoomResponse>>
{
    public async Task<ICollection<RoomResponse>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllRooms(cancellationToken);
        var response = new List<RoomResponse>();
        foreach (var room in rooms)
        {
            var hotelDiscounts = await hotelDiscountRepository.GetDiscountsByTypeId(room.Hotel.Id, cancellationToken);
            var roomDiscounts = await roomDiscountRepository.GetDiscountsByTypeId(room.Id, cancellationToken);
            int totalDiscountPercent = 0;
            foreach (var discount in roomDiscounts)
            {
                if (discount.From < DateTime.Now && discount.To > DateTime.Now)
                    totalDiscountPercent += discount.DiscountPercent;
            }
            foreach (var discount in hotelDiscounts)
            {
                if (discount.From < DateTime.Now && discount.To > DateTime.Now)
                    totalDiscountPercent += discount.DiscountPercent;
            }
            response.Add(new RoomResponse
            {
                Id = room.Id,
                RoomName = room.RoomName,
                RoomType = room.RoomType.ToString(),
                Price = room.Price,
                HotelId = room.Hotel.Id,
                RoomImages = (await imageRepository.GetRoomImagesByRoomId(room.Id, cancellationToken))
                    .Select(i => fileService.GetFileUrl("images", i.FileName)).ToList(),
                DiscountPercent = totalDiscountPercent <= 100 ? totalDiscountPercent : 100,
            });
        }
        return response;
    }
}
