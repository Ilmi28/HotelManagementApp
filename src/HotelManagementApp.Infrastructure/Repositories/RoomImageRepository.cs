using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.RoomModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class RoomImageRepository(AppDbContext context) : IRoomImageRepository
{
    public async Task AddRoomImage(RoomImage roomImage, CancellationToken ct)
    {
        context.Attach(roomImage.Room);
        await context.RoomImages.AddAsync(roomImage, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<RoomImage>> GetRoomImagesByRoomId(int roomId, CancellationToken ct)
    {
        return await context.RoomImages
            .AsNoTracking()
            .Include(x => x.Room)
            .Where(x => x.Room.Id == roomId)
            .ToListAsync(ct);
    }

    public async Task RemoveRoomImagesByRoomId(int roomId, CancellationToken ct)
    {
        await context.RoomImages
            .Include(x => x.Room)
            .Where(x => x.Room.Id == roomId)
            .ExecuteDeleteAsync(ct);
    }
}
