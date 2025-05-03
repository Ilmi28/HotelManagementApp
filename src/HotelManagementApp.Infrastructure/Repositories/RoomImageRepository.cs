using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class RoomImageRepository(AppDbContext context) : IRoomImageRepository
{
    public async Task AddRoomImage(HotelRoomImage roomImage, CancellationToken ct)
    {
        context.Attach(roomImage.Room);
        await context.HotelRoomImages.AddAsync(roomImage, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<HotelRoomImage>> GetRoomImagesByRoomId(int roomId, CancellationToken ct)
    {
        return await context.HotelRoomImages
            .AsNoTracking()
            .Include(x => x.Room)
            .Where(x => x.Room.Id == roomId)
            .ToListAsync(ct);
    }

    public async Task RemoveRoomImagesByRoomId(int roomId, CancellationToken ct)
    {
        var roomImages = await context.HotelRoomImages
                            .Include(x => x.Room)
                            .Where(x => x.Room.Id == roomId).ToListAsync();
        foreach (var roomImage in roomImages)
        {
            context.HotelRoomImages.Remove(roomImage);
        }
        await context.SaveChangesAsync();
    }
}
