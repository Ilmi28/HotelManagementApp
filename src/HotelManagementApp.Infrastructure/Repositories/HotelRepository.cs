using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class HotelRepository(AppDbContext context) : IHotelRepository
{
    public async Task AddHotel(Hotel model, CancellationToken ct)
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

    public async Task<ICollection<Hotel>> GetAllHotels(CancellationToken ct)
    {
        return await context.Hotels
            .AsNoTracking()
            .Include(h => h.City)
            .ToListAsync(ct);
    }

    public async Task<Hotel?> GetHotelById(int id, CancellationToken ct)
    {
        return await context.Hotels.AsNoTracking()
            .Include(h => h.City)
            .FirstOrDefaultAsync(h => h.Id == id, ct);
    }

    public async Task UpdateHotel(Hotel hotel, CancellationToken ct)
    {
        var model = await context.Hotels
            .FirstOrDefaultAsync(x => x.Id == hotel.Id, ct);
        if (model != null)
        {
            model.Name = hotel.Name;
            model.Address = hotel.Address;
            model.City = hotel.City;
            model.PhoneNumber = hotel.PhoneNumber;
            model.Email = hotel.Email;
            model.Description = hotel.Description;
            context.Hotels.Update(model);
            await context.SaveChangesAsync(ct);
        }
    }
}
