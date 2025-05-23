﻿using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.HotelRepositories;

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
        var hotelImage = await context.HotelImages
                            .Include(x => x.Hotel)
                            .FirstOrDefaultAsync(x => x.Hotel.Id == hotelId, ct);
        if (hotelImage != null)
        {
            context.HotelImages.Remove(hotelImage);
            await context.SaveChangesAsync(ct);
        }
    }

}
