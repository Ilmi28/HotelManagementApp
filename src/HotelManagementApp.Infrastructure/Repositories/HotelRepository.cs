using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class HotelRepository(AppDbContext context) : IHotelRepository
{
    public async Task AddHotel(HotelModel model, CancellationToken ct)
    {
        await context.Hotels.AddAsync(model, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveHotel(int id, CancellationToken ct)
    {
        await context.Hotels
            .Where(h => h.Id == id)
            .ExecuteDeleteAsync(ct);
    }

    public async Task<ICollection<HotelModel>> GetAllHotels(CancellationToken ct)
    {
        return await context.Hotels
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<HotelModel?> GetHotelById(int id, CancellationToken ct)
    {
        return await context.Hotels.AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == id, ct);
    }

    public async Task UpdateHotel(HotelModel hotel, CancellationToken ct)
    {
        await context.Hotels
            .Where(x => x.Id == hotel.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.Name, hotel.Name)
                .SetProperty(x => x.Address, hotel.Address)
                .SetProperty(x => x.City, hotel.City)
                .SetProperty(x => x.Country, hotel.Country)
                .SetProperty(x => x.PhoneNumber, hotel.PhoneNumber)
                .SetProperty(x => x.Email, hotel.Email)
                .SetProperty(x => x.Description, hotel.Description)
                , ct);
    }
}
