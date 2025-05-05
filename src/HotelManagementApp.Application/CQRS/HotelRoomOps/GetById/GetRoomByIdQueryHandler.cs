using HotelManagementApp.Application.Responses.RoomResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.GetById;

public class GetRoomByIdQueryHandler(
    IRoomRepository roomRepository, 
    IRoomImageRepository imageRepository,
    IFileService fileService,
    IHotelDiscountRepository hotelDiscountRepository,
    IRoomDiscountRepository roomDiscountRepository) : IRequestHandler<GetRoomByIdQuery, RoomResponse>
{
    public async Task<RoomResponse> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var roomModel = await roomRepository.GetRoomById(request.RoomId, cancellationToken)
            ?? throw new RoomNotFoundException($"Room with id {request.RoomId} not found");
        var hotelDiscounts = await hotelDiscountRepository.GetDiscountsByTypeId(roomModel.Hotel.Id, cancellationToken);
        var roomDiscounts = await roomDiscountRepository.GetDiscountsByTypeId(roomModel.Id, cancellationToken);
        int totalDiscountPercent = 0;
        foreach (var discount in hotelDiscounts)
        {
            if (discount.From < DateTime.Now && discount.To > DateTime.Now)
                totalDiscountPercent += discount.DiscountPercent;
        }
        foreach (var discount in roomDiscounts)
        {
            if (discount.From < DateTime.Now && discount.To > DateTime.Now)
                totalDiscountPercent += discount.DiscountPercent;
        }
        return new RoomResponse
        {
            RoomName = roomModel.RoomName,
            RoomType = roomModel.RoomType.ToString(),
            Price = roomModel.Price,
            HotelId = roomModel.Hotel.Id,
            RoomImages = (await imageRepository.GetRoomImagesByRoomId(roomModel.Id, cancellationToken))
                .Select(i => fileService.GetFileUrl("images", i.FileName)).ToList(),
            DiscountPercent = totalDiscountPercent <= 100 ? totalDiscountPercent : 100,
        };

    }
}
