using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.LoyaltyPointsRepositories;

public class LoyaltyRewardRepository(AppDbContext context) : ILoyaltyRewardsRepository
{
    public async Task<LoyaltyReward?> GetLoyaltyRewardById(int id, CancellationToken cancellationToken = default)
    {
        return await context.LoyaltyRewards
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<LoyaltyReward>> GetAllLoyaltyRewards(CancellationToken cancellationToken = default)
    {
        return await context.LoyaltyRewards
            .ToListAsync(cancellationToken);
    }

    public async Task AddLoyaltyReward(LoyaltyReward loyaltyReward, CancellationToken cancellationToken = default)
    {
        await context.LoyaltyRewards.AddAsync(loyaltyReward, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateLoyaltyReward(LoyaltyReward loyaltyReward, CancellationToken cancellationToken = default)
    {
        context.LoyaltyRewards.Update(loyaltyReward);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveLoyaltyReward(LoyaltyReward loyaltyReward, CancellationToken cancellationToken = default)
    {
        context.LoyaltyRewards.Remove(loyaltyReward);
        await context.SaveChangesAsync(cancellationToken);
    }
}