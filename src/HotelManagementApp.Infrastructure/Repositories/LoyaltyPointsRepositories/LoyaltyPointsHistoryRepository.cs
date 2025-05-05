using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.LoyaltyPointsRepositories;

public class LoyaltyPointsHistoryRepository(AppDbContext context) : ILoyaltyPointsHistoryRepository
{
    public async Task AddPointsLog(LoyaltyPointsLog log)
    {
        await context.LoyaltyPointsHistory.AddAsync(log);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<LoyaltyPointsLog>> GetLoyaltyPointsHistoryByGuestId(string guestId)
    {
        return await context.LoyaltyPointsHistory
            .AsNoTracking()
            .Where(x => x.UserId == guestId)
            .ToListAsync();
    }
}
