using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.LoyaltyPointsRepositories;

public class LoyaltyPointsRepository(AppDbContext context) : ILoyaltyPointsRepository
{

    public async Task AddLoyaltyPoints(LoyaltyPoints loyaltyPoints, CancellationToken cancellationToken)
    {
        await context.LoyaltyPoints.AddAsync(loyaltyPoints, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateLoyaltyPoints(LoyaltyPoints loyaltyPoints, CancellationToken cancellationToken)
    {
        context.LoyaltyPoints.Update(loyaltyPoints);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<LoyaltyPoints?> GetLoyaltyPointsByGuestId(string id, CancellationToken cancellationToken)
    {
        return await context.LoyaltyPoints
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.GuestId == id, cancellationToken);
    }

    public async Task<ICollection<LoyaltyPoints>> GetAllLoyaltyPoints(CancellationToken cancellationToken)
    {
        return await context.LoyaltyPoints
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
