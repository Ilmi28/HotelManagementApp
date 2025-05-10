using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.OrderRepositories;

public class CancelledOrderRepository(AppDbContext context) : ICancelledOrderRepository
{
    public async Task AddCancelledOrder(CancelledOrder order, CancellationToken ct)
    {
        context.Attach(order.Order);
        await context.CancelledOrders.AddAsync(order, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<CancelledOrder>> GetCancelledOrdersByGuestId(string guestId, CancellationToken ct)
    {
        return await context.CancelledOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .Where(x => x.Order.UserId == guestId)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<ICollection<CancelledOrder>> GetCancelledOrders(CancellationToken ct)
    {
        return await context.CancelledOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .ToListAsync(cancellationToken: ct);
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

    public async Task<CancelledOrder?> GetCancelledOrderByOrderId(int id, CancellationToken ct)
    {
        return await context.CancelledOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.Order.Id == id, ct);
    }
}
