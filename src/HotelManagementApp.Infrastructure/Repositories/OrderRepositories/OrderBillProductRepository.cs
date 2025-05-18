using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.OrderRepositories;

public class OrderBillProductRepository(AppDbContext context) : IOrderBillProductRepository
{
    public async Task AddOrderBillProduct(OrderBillProduct orderBillProduct, CancellationToken ct)
    {
        await context.OrderProducts.AddAsync(orderBillProduct, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteOrderBillProduct(OrderBillProduct orderBillProduct, CancellationToken ct)
    {
        context.OrderProducts.Remove(orderBillProduct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<OrderBillProduct>> GetOrderBillProductsByOrderId(int orderId, CancellationToken ct)
    {
        return await context.OrderProducts
            .AsNoTracking()
            .Include(x => x.Order)
            .Where(x => x.OrderId == orderId).ToListAsync(ct);
    }
}