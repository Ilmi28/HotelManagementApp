using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.OrderRepositories;

public class CompletedOrderRepository(AppDbContext context) : ICompletedOrderRepository
{
    public async Task AddCompletedOrder(CompletedOrder order, CancellationToken ct)
    {
        await context.CompletedOrders.AddAsync(order);
        await context.SaveChangesAsync();
    }

    public Task DeleteCompletedOrder(CompletedOrder order, CancellationToken ct)
    {
        context.CompletedOrders.Remove(order);
        return context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<CompletedOrder>> GetAllCompletedOrders(CancellationToken ct)
    {
        return await context.CompletedOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .ToListAsync();
    }

    public Task<CompletedOrder?> GetCompletedOrderByOrderId(int id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<CompletedOrder>> GetCompletedOrdersByGuestId(string guestId, CancellationToken ct)
    {
        return await context.CompletedOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .Where(x => x.Order.UserId == guestId)
            .ToListAsync();
    }
}
