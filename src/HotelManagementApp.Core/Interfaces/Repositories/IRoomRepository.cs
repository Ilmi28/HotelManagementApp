using HotelManagementApp.Core.Models.RoomModels;

namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface IRoomRepository
{
    Task AddRoom(RoomModel model, CancellationToken ct);
    Task RemoveRoom(int id, CancellationToken ct);
    Task<ICollection<RoomModel>> GetAllRooms(CancellationToken ct);
    Task<RoomModel?> GetRoomById(int id, CancellationToken ct);
    Task UpdateRoom(RoomModel room, CancellationToken ct);
    Task<ICollection<RoomModel>> GetRoomsByHotelId(int hotelId, CancellationToken ct);
    Task<ICollection<RoomType>> GetRoomTypes(CancellationToken ct);
}
