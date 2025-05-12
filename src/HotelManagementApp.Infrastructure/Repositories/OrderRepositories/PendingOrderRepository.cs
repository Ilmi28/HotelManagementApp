using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.OrderRepositories;

public class PendingOrderRepository(AppDbContext context) : IPendingOrderRepository
{
    public async Task AddPendingOrder(PendingOrder order, CancellationToken ct)
    {
        context.Attach(order.Order);
        await context.PendingOrders.AddAsync(order, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<PendingOrder?> GetPendingOrderById(int id, CancellationToken ct)
    {
        return await context.PendingOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<ICollection<PendingOrder>> GetPendingOrders(CancellationToken ct)
    {
        return await context.PendingOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .ToListAsync(ct);
    }

    public async Task DeletePendingOrder(PendingOrder pendingOrder, CancellationToken ct)
    {
        context.PendingOrders.Remove(pendingOrder);
        await context.SaveChangesAsync(ct);
    }

    public async Task<PendingOrder?> GetPendingOrderByOrderId(int orderId, CancellationToken ct)
    {
        return await context.PendingOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.Order.Id == orderId, ct);
    }
}
