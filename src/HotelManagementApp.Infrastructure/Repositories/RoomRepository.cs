using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.RoomModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class RoomRepository(AppDbContext context) : IRoomRepository
{
    public async Task AddRoom(RoomModel model, CancellationToken ct)
    {
        context.Attach(model.Hotel);
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
        var room = await context.Rooms.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (room != null)
        {
            context.Rooms.Remove(room);
            await context.SaveChangesAsync(ct);
        }
    }

    public async Task UpdateRoom(RoomModel room, CancellationToken ct)
    {
        var model = await context.Rooms.FirstOrDefaultAsync(r => r.Id == room.Id, ct);
        if (model != null)
        {
            model.RoomName = room.RoomName;
            model.RoomType = room.RoomType;
            model.Price = room.Price;

            await context.SaveChangesAsync(ct);
        }
    }
}
