using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface IRoomRepository
{
    Task AddRoom(HotelRoom model, CancellationToken ct);
    Task RemoveRoom(HotelRoom room, CancellationToken ct);
    Task<ICollection<HotelRoom>> GetAllRooms(CancellationToken ct);
    Task<HotelRoom?> GetRoomById(int id, CancellationToken ct);
    Task UpdateRoom(HotelRoom room, CancellationToken ct);
    Task<ICollection<HotelRoom>> GetRoomsByHotelId(int hotelId, CancellationToken ct);
    Task<ICollection<HotelRoomType>> GetRoomTypes(CancellationToken ct);
}
