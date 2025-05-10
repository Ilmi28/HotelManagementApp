using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.HotelRepositories;

public class HotelServiceRepository(AppDbContext context) : IHotelServiceRepository
{
    public async Task AddHotelService(HotelService hotelService, CancellationToken cancellationToken)
    {
        context.Attach(hotelService.Hotel);
        await context.HotelServices.AddAsync(hotelService, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ICollection<HotelService>> GetHotelServicesByHotel(int hotelId, CancellationToken cancellationToken)
    {
        return await context.HotelServices
            .AsNoTracking()
            .Include(x => x.Hotel)
            .Where(h => h.Hotel.Id == hotelId)
            .ToListAsync(cancellationToken);
    }

    public async Task<HotelService?> GetHotelServiceById(int id, CancellationToken cancellationToken)
    {
        return await context.HotelServices
            .AsNoTracking()
            .Include(x => x.Hotel)
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
    }

    public async Task DeleteHotelService(HotelService hotelService, CancellationToken cancellationToken)
    {
        context.HotelServices.Remove(hotelService);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateHotelService(HotelService hotelService, CancellationToken cancellationToken)
    {
        context.Attach(hotelService.Hotel);
        context.HotelServices.Update(hotelService);
        await context.SaveChangesAsync(cancellationToken);
    }
}
