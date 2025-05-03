using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface IRoomImageRepository
{
    Task AddRoomImage(HotelRoomImage roomImage, CancellationToken ct);
    Task<ICollection<HotelRoomImage>> GetRoomImagesByRoomId(int roomId, CancellationToken ct);
    Task RemoveRoomImagesByRoomId(int roomId, CancellationToken ct);
}
