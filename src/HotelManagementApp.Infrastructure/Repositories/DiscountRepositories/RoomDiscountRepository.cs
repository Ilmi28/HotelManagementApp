using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.DiscountRepositories;

public class RoomDiscountRepository(AppDbContext context) : IRoomDiscountRepository
{
    public async Task AddDiscount(RoomDiscount discount, CancellationToken ct = default)
    {
        context.Attach(discount.Room);
        await context.RoomDiscounts.AddAsync(discount, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<RoomDiscount>> GetAllDiscounts(CancellationToken ct = default)
    {
        return await context.RoomDiscounts
            .AsNoTracking()
            .Include(x => x.Room)
            .ToListAsync(ct);
    }

    public async Task<RoomDiscount?> GetDiscountById(int id, CancellationToken ct = default)
    {
        return await context.RoomDiscounts
            .AsNoTracking()
            .Include(x => x.Room)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<ICollection<RoomDiscount>> GetDiscountsByTypeId(int id, CancellationToken ct = default)
    {
        return await context.RoomDiscounts
            .AsNoTracking()
            .Include(x => x.Room)
            .Where(x => x.Room.Id == id)
            .ToListAsync(ct);
    }

    public async Task RemoveDiscount(RoomDiscount discount, CancellationToken ct = default)
    {
        context.RoomDiscounts.Remove(discount);
        await context.SaveChangesAsync(ct);
    }

}
