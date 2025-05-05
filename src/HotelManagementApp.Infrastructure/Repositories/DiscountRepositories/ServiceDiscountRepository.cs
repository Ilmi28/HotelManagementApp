using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.DiscountRepositories;

public class ServiceDiscountRepository(AppDbContext context) : IServiceDiscountRepository
{
    public async Task AddDiscount(ServiceDiscount discount, CancellationToken ct = default)
    {
        context.Attach(discount.Service);
        await context.ServiceDiscounts.AddAsync(discount, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<ServiceDiscount>> GetAllDiscounts(CancellationToken ct = default)
    {
        return await context.ServiceDiscounts
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<ServiceDiscount?> GetDiscountById(int id, CancellationToken ct = default)
    {
        return await context.ServiceDiscounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<ICollection<ServiceDiscount>> GetDiscountsByTypeId(int id, CancellationToken ct = default)
    {
        return await context.ServiceDiscounts
            .AsNoTracking()
            .Include(x => x.Service)
            .Where(x => x.Service.Id == id)
            .ToListAsync(ct);
    }

    public async Task RemoveDiscount(ServiceDiscount discount, CancellationToken ct = default)
    {
        context.ServiceDiscounts.Remove(discount);
        await context.SaveChangesAsync(ct);
    }
}
