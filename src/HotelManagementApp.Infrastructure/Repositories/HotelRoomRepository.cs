using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class HotelRoomRepository(AppDbContext context) : IRoomRepository
{
    public async Task AddRoom(HotelRoom model, CancellationToken ct)
    {
        context.Attach(model.Hotel);
        await context.HotelRooms.AddAsync(model, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<HotelRoom>> GetAllRooms(CancellationToken ct)
    {
        return await context.HotelRooms
            .AsNoTracking()
            .Include(r => r.Hotel)
            .ToListAsync(ct);
    }

    public async Task<HotelRoom?> GetRoomById(int id, CancellationToken ct)
    {
        return await context.HotelRooms.AsNoTracking()
            .Include(r => r.Hotel)
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == id, ct);
    }

    public async Task<ICollection<HotelRoomType>> GetRoomTypes(CancellationToken ct)
    {
        return await context.HotelRoomTypes.AsNoTracking().ToListAsync();
    }

    public async Task<ICollection<HotelRoom>> GetRoomsByHotelId(int hotelId, CancellationToken ct)
    {
        return await context.HotelRooms
            .AsNoTracking()
            .Include(r => r.Hotel)
            .Where(r => r.Hotel.Id == hotelId)
            .ToListAsync(ct);
    }

    public async Task RemoveRoom(HotelRoom room, CancellationToken ct)
    {
        context.HotelRooms.Remove(room);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateRoom(HotelRoom room, CancellationToken ct)
    {
        context.Attach(room.Hotel);
        context.HotelRooms.Update(room);
        await context.SaveChangesAsync(ct);
    }
}
