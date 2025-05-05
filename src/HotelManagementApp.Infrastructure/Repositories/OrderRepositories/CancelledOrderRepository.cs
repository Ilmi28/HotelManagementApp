using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.OrderRepositories;

public class CancelledOrderRepository(AppDbContext context) : ICancelledOrderRepository
{
    public async Task AddCancelledOrder(CancelledOrder order, CancellationToken ct)
    {
        await context.CancelledOrders.AddAsync(order);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<CancelledOrder>> GetCancelledOrdersByGuestId(string guestId, CancellationToken ct)
    {
        return await context.CancelledOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .Where(x => x.Order.UserId == guestId)
            .ToListAsync();
    }

    public async Task<ICollection<CancelledOrder>> GetCancelledOrders(CancellationToken ct)
    {
        return await context.CancelledOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .ToListAsync();
    }

    public async Task<CancelledOrder?> GetCancelledOrderById(int id, CancellationToken ct)
    {
        return await context.CancelledOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task DeleteCancelledOrder(CancelledOrder order, CancellationToken ct)
    {
        context.CancelledOrders.Remove(order);
        await context.SaveChangesAsync(ct);
    }
}
