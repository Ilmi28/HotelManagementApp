using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.LoyaltyPointsRepositories;

public class LoyaltyPointsRepository(AppDbContext context) : ILoyaltyPointsRepository
{

    public async Task AddLoyaltyPoints(LoyaltyPoints loyaltyPoints)
    {
        await context.LoyaltyPoints.AddAsync(loyaltyPoints);
        await context.SaveChangesAsync();
    }

    public async Task UpdateLoyaltyPoints(LoyaltyPoints loyaltyPoints)
    {
        context.LoyaltyPoints.Update(loyaltyPoints);
        await context.SaveChangesAsync();
    }

    public async Task<LoyaltyPoints?> GetLoyaltyPointsByGuestId(string id)
    {
        return await context.LoyaltyPoints
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.GuestId == id);
    }

    public async Task<ICollection<LoyaltyReward>> GetLoyaltyRewards()
    {
        return await context.LoyaltyRewards
            .AsNoTracking()
            .ToListAsync();
    }


}
