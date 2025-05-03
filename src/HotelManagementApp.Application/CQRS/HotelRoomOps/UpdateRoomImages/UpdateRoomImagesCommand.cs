using MediatR;
using Microsoft.AspNetCore.Http;

namespace HotelManagementApp.Application.CQRS.HotelRoomOps.UpdateRoomImages;

public class UpdateRoomImagesCommand : IRequest
{
    public int RoomId { get; set; }
    public List<IFormFile> RoomImages { get; set; } = new List<IFormFile>();
}
