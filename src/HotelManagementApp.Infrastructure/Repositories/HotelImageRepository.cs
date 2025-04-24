using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class HotelImageRepository(AppDbContext context)
{
    public async Task AddHotelImage(HotelImage hotelImage, CancellationToken ct)
    {
        await context.HotelImages.AddAsync(hotelImage, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<HotelImage>> GetAllHotelImages(CancellationToken ct)
    {
        return await context.HotelImages
            .AsNoTracking()
            .ToListAsync(ct);
    }

}
