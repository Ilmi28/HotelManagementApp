using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.DiscountRepositories;

public class HotelDiscountRepository(AppDbContext context) : IHotelDiscountRepository
{
    public async Task AddDiscount(HotelDiscount discount, CancellationToken ct = default)
    {
        context.Attach(discount.Hotel);
        await context.HotelDiscounts.AddAsync(discount, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<HotelDiscount>> GetAllDiscounts(CancellationToken ct = default)
    {
        return await context.HotelDiscounts
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<HotelDiscount?> GetDiscountById(int id, CancellationToken ct = default)
    {
        return await context.HotelDiscounts
            .AsNoTracking()
            .Include(x => x.Hotel)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<ICollection<HotelDiscount>> GetDiscountsByTypeId(int id, CancellationToken ct = default)
    {
        return await context.HotelDiscounts
            .AsNoTracking()
            .Include(x => x.Hotel)
            .Where(x => x.Hotel.Id == id)
            .ToListAsync(ct);
    }

    public async Task RemoveDiscount(HotelDiscount discount, CancellationToken ct = default)
    {
        context.HotelDiscounts.Remove(discount);
        await context.SaveChangesAsync(ct);
    }
}
