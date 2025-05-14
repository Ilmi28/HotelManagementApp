using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.LoyaltyPointsRepositories;

public class LoyaltyPointsHistoryRepository(AppDbContext context) : ILoyaltyPointsHistoryRepository
{
    public async Task AddPointsLog(LoyaltyPointsLog log, CancellationToken cancellationToken = default)
    {
        await context.LoyaltyPointsHistory.AddAsync(log, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ICollection<LoyaltyPointsLog>> GetLoyaltyPointsHistoryByGuestId(string guestId, CancellationToken cancellationToken = default)
    {
        return await context.LoyaltyPointsHistory
            .AsNoTracking()
            .Where(x => x.UserId == guestId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<LoyaltyPointsLog>> GetAllLoyaltyPointsHistory(CancellationToken cancellationToken)
    {
        return await context.LoyaltyPointsHistory
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
