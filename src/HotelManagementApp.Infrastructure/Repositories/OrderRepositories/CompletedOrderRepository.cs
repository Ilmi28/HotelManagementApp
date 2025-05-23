﻿using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.OrderRepositories;

public class CompletedOrderRepository(AppDbContext context) : ICompletedOrderRepository
{
    public async Task AddCompletedOrder(CompletedOrder order, CancellationToken ct)
    {
        context.Attach(order);
        await context.CompletedOrders.AddAsync(order, ct);
        await context.SaveChangesAsync(ct);
    }

    public Task DeleteCompletedOrder(CompletedOrder order, CancellationToken ct)
    {
        context.CompletedOrders.Remove(order);
        return context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<CompletedOrder>> GetCompletedOrders(CancellationToken ct)
    {
        return await context.CompletedOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<CompletedOrder?> GetCompletedOrderByOrderId(int id, CancellationToken ct)
    {
        return await context.CompletedOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.Order != null && x.Order.Id == id, ct);
    }

    public async Task<ICollection<CompletedOrder>> GetCompletedOrdersByGuestId(string guestId, CancellationToken ct)
    {
        return await context.CompletedOrders
            .AsNoTracking()
            .Include(x => x.Order)
            .Where(x => x.Order != null && x.Order.UserId == guestId)
            .ToListAsync(cancellationToken: ct);
    }
}
