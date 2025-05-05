using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.Add;

public class AddRoomCommandHandler(IRoomRepository roomRepository, 
    IHotelRepository hotelRepository) : IRequestHandler<AddRoomCommand>
{
    public async Task Handle(AddRoomCommand request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var roomModel = new HotelRoom
        {
            RoomName = request.RoomName,
            RoomType = request.RoomType,
            Price = request.Price,
            Description = request.Description,
            Hotel = hotel
        };
        await roomRepository.AddRoom(roomModel, cancellationToken);
    }
}
