using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.OrderRepositories;

public class ConfirmedOrderRepository(AppDbContext context) : IConfirmedOrderRepository
{
    public async Task AddConfirmedOrder(ConfirmedOrder order, CancellationToken ct)
    {
        await context.ConfirmedOrders.AddAsync(order);
        await context.SaveChangesAsync();
    }

    public Task DeleteConfirmedOrder(ConfirmedOrder order, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<ConfirmedOrder>> GetAllConfirmedOrders(CancellationToken ct)
    {
        return await context.ConfirmedOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .ToListAsync();
    }

    public async Task<ConfirmedOrder?> GetConfirmedOrderByOrderId(int id, CancellationToken ct)
    {
        return await context.ConfirmedOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.Order.Id == id);
    }

    public async Task<ICollection<ConfirmedOrder>> GetConfirmedOrdersByGuestId(string guestId, CancellationToken ct)
    {
        return await context.ConfirmedOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .Where(x => x.Order.UserId == guestId)
            .ToListAsync();
    }
}
