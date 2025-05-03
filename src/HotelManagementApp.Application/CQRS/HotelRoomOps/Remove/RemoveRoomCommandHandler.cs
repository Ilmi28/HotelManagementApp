using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.Remove;

public class RemoveRoomCommandHandler(IRoomRepository roomRepository) : IRequestHandler<RemoveRoomCommand>
{
    public async Task Handle(RemoveRoomCommand request, CancellationToken cancellationToken)
    {
       var roomModel = await roomRepository.GetRoomById(request.RoomId, cancellationToken)
            ?? throw new RoomNotFoundException($"Room with id {request.RoomId} not found");
        await roomRepository.RemoveRoom(roomModel, cancellationToken);
    }
}
