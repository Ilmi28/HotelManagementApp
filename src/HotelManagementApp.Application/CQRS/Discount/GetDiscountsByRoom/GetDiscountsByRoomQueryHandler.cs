using HotelManagementApp.Application.Responses.DiscountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.GetDiscountsByRoom;

public class GetDiscountsByRoomQueryHandler(
    IRoomRepository roomRepository, 
    IRoomDiscountRepository discountRepository) : IRequestHandler<GetDiscountsByRoomQuery, ICollection<RoomDiscountResponse>>
{
    public async Task<ICollection<RoomDiscountResponse>> Handle(GetDiscountsByRoomQuery request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetRoomById(request.RoomId, cancellationToken)
            ?? throw new RoomNotFoundException($"Room with id {request.RoomId} not found");
        var discounts = await discountRepository.GetDiscountsByTypeId(request.RoomId, cancellationToken);
        return discounts
            .Select(x => new RoomDiscountResponse
            {
                Id = x.Id,
                RoomId = x.Room.Id,
                DiscountPercent = x.DiscountPercent,
                From = x.From,
                To = x.To
            })
            .ToList();
    }
}
