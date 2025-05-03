using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.Remove;

public class RemoveRoomCommand : IRequest
{
    public required int RoomId { get; set; }
}
