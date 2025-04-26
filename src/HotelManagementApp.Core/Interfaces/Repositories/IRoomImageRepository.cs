using HotelManagementApp.Core.Models.RoomModels;

namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface IRoomImageRepository
{
    Task AddRoomImage(RoomImage roomImage, CancellationToken ct);
    Task<ICollection<RoomImage>> GetRoomImagesByRoomId(int roomId, CancellationToken ct);
    Task RemoveRoomImagesByRoomId(int roomId, CancellationToken ct);
}
