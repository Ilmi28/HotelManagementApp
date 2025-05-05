using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.HotelRepositories;

public class HotelParkingRepository(AppDbContext context) : IHotelParkingRepository
{
    public async Task AddHotelParking(HotelParking hotelParking, CancellationToken ct)
    {
        context.Attach(hotelParking.Hotel);
        await context.HotelParkings.AddAsync(hotelParking, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteHotelParking(HotelParking hotelParking, CancellationToken ct)
    {
        context.HotelParkings.Remove(hotelParking);
        await context.SaveChangesAsync(ct);
    }

    public async Task<HotelParking?> GetHotelParkingById(int id, CancellationToken ct)
    {
        return await context.HotelParkings
            .Include(x => x.Hotel)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<ICollection<HotelParking>> GetHotelParkingsByHotelId(int hotelId, CancellationToken ct)
    {
        return await context.HotelParkings
            .Include(x => x.Hotel)
            .AsNoTracking()
            .Where(hp => hp.Hotel.Id == hotelId)
            .ToListAsync(ct);
    }

    public async Task UpdateHotelParking(HotelParking hotelParking, CancellationToken ct)
    {
        context.Attach(hotelParking.Hotel);
        context.HotelParkings.Update(hotelParking);
        await context.SaveChangesAsync(ct);
    }
}
