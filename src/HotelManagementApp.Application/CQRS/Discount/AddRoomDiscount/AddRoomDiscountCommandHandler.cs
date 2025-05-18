using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.AddRoomDiscount;

public class AddRoomDiscountCommandHandler(
    IRoomRepository roomRepository, 
    IRoomDiscountRepository discountRepository) : IRequestHandler<AddRoomDiscountCommand>
{
    public async Task Handle(AddRoomDiscountCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetRoomById(request.RoomId, cancellationToken)
            ?? throw new RoomNotFoundException($"Room with id {request.RoomId} not found");
        var discount = new RoomDiscount
        {
            Room = room,
            DiscountPercent = request.DiscountPercent,
            From = request.From,
            To = request.To
        };
        await discountRepository.AddDiscount(discount, cancellationToken);
    }
}
