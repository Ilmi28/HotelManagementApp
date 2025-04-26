using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class HotelImageRepository(AppDbContext context) : IHotelImageRepository
{
    public async Task AddHotelImage(HotelImage hotelImage, CancellationToken ct)
    {
        context.Attach(hotelImage.Hotel);
        await context.HotelImages.AddAsync(hotelImage, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<HotelImage>> GetHotelImagesByHotelId(int hotelId, CancellationToken ct)
    {
        return await context.HotelImages
            .AsNoTracking()
            .Include(x => x.Hotel)
            .Where(x => x.Hotel.Id == hotelId)
            .ToListAsync(ct);
    }

    public async Task RemoveHotelImagesByHotelId(int hotelId, CancellationToken ct)
    {
        await context.HotelImages
            .Include(x => x.Hotel)
            .Where(x => x.Hotel.Id == hotelId)
            .ExecuteDeleteAsync(ct);
    }

}
