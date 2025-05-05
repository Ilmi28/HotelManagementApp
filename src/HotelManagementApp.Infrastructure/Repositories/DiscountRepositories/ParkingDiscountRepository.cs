using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.DiscountRepositories;

public class ParkingDiscountRepository(AppDbContext context) : IParkingDiscountRepository
{
    public async Task AddDiscount(ParkingDiscount discount, CancellationToken ct = default)
    {
        context.Attach(discount.Parking);
        await context.ParkingDiscounts.AddAsync(discount, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<ParkingDiscount>> GetAllDiscounts(CancellationToken ct = default)
    {
        return await context.ParkingDiscounts
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<ParkingDiscount?> GetDiscountById(int id, CancellationToken ct = default)
    {
        return await context.ParkingDiscounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<ICollection<ParkingDiscount>> GetDiscountsByTypeId(int id, CancellationToken ct = default)
    {
        return await context.ParkingDiscounts
            .AsNoTracking()
            .Include(x => x.Parking)
            .Where(x => x.Parking.Id == id)
            .ToListAsync(ct);
    }

    public async Task RemoveDiscount(ParkingDiscount discount, CancellationToken ct = default)
    {
        context.ParkingDiscounts.Remove(discount);
        await context.SaveChangesAsync(ct);
    }
}
