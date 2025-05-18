using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.OrderRepositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task AddOrder(Order order, CancellationToken ct = default)
    {
        await context.Orders.AddAsync(order, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<Order?> GetOrderById(int id, CancellationToken ct = default)
    {
        return await context.Orders
            .AsNoTracking()
            .Include(x => x.Reservations)
            .Include(x => x.OrderDetails)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: ct);
    }

    public async Task<ICollection<Order>> GetAllOrders(CancellationToken ct = default)
    {
        return await context.Orders
            .AsNoTracking()
            .Include(x => x.Reservations)
            .Include(x => x.OrderDetails)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<ICollection<Order>> GetOrdersByGuestId(string guestId, CancellationToken ct = default)
    {
        return await context.Orders
            .AsNoTracking()
            .Include(x => x.Reservations)
            .Include(x => x.OrderDetails)
            .Where(x => x.UserId == guestId)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task UpdateOrder(Order order, CancellationToken ct = default)
    {
        var trackedEntity = context.ChangeTracker.Entries<Order>()
            .FirstOrDefault(e => e.Entity.Id == order.Id);
        if (trackedEntity != null)
        {
            context.Entry(trackedEntity.Entity).State = EntityState.Detached;
        }

        // Reload fresh from DB without tracking
        var freshOrder = await context.Orders.AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == order.Id, ct);
        if (freshOrder == null)
            throw new InvalidOperationException("Order not found");

        // Apply updates manually if needed, or just attach order
        context.Attach(order).State = EntityState.Modified;
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteOrder(Order order, CancellationToken ct = default)
    {
        context.Orders.Remove(order);
        await context.SaveChangesAsync(ct);
    }

}
