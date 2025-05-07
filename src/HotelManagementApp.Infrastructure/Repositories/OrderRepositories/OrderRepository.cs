using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.OrderRepositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task AddOrder(Order order)
    {
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await context.Orders
            .AsNoTracking()
            .Include(x => x.Reservations)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Order>> GetAllOrders()
    {
        return await context.Orders
            .AsNoTracking()
            .Include(x => x.Reservations)
            .ToListAsync();
    }

    public async Task<ICollection<Order>> GetOrdersByGuestId(string guestId)
    {
        return await context.Orders
            .AsNoTracking()
            .Include(x => x.Reservations)
            .Where(x => x.UserId == guestId)
            .ToListAsync();
    }

    public async Task UpdateOrder(Order order)
    {
        context.Orders.Update(order);
        await context.SaveChangesAsync();
    }

    public async Task DeleteOrder(Order order)
    {
        context.Orders.Remove(order);
        await context.SaveChangesAsync();
    }

}
