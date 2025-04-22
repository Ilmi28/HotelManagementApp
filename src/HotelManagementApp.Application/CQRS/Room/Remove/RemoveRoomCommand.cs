using MediatR;

namespace HotelManagementApp.Application.CQRS.Room.Remove;

public class RemoveRoomCommand : IRequest
{
    public required int RoomId { get; set; }
}
