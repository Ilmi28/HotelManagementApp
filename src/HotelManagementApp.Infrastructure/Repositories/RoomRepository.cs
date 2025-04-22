using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.RoomModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class RoomRepository(AppDbContext context) : IRoomRepository
{
    public async Task AddRoom(RoomModel model, CancellationToken ct)
    {
        await context.Rooms.AddAsync(model, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<RoomModel>> GetAllRooms(CancellationToken ct)
    {
        return await context.Rooms
            .AsNoTracking()
            .Include(r => r.Hotel)
            .ToListAsync(ct);
    }

    public async Task<RoomModel?> GetRoomById(int id, CancellationToken ct)
    {
        return await context.Rooms.AsNoTracking()
            .Include(r => r.Hotel)
            .FirstOrDefaultAsync(h => h.Id == id, ct);
    }

    public async Task<ICollection<RoomType>> GetRoomTypes(CancellationToken ct)
    {
        return await context.RoomTypes.ToListAsync();
    }

    public async Task<ICollection<RoomModel>> GetRoomsByHotelId(int hotelId, CancellationToken ct)
    {
        return await context.Rooms
            .AsNoTracking()
            .Include(r => r.Hotel)
            .Where(r => r.Hotel.Id == hotelId)
            .ToListAsync(ct);
    }

    public async Task RemoveRoom(int id, CancellationToken ct)
    {
        await context.Rooms
            .Where(h => h.Id == id)
            .ExecuteDeleteAsync(ct);
    }

    public async Task UpdateRoom(RoomModel room, CancellationToken ct)
    {
        await context.Rooms
            .Where(h => h.Id == room.Id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(r => r.RoomName, room.RoomName)
                .SetProperty(r => r.RoomType, room.RoomType)
                .SetProperty(r => r.Price, room.Price), ct);
    }
}
