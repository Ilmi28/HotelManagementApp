using HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.LoyaltyPointsRepositories;

public class LoyaltyRewardUserRepository(AppDbContext context) : ILoyaltyRewardUserRepository
{
    public async Task AddLoyaltyRewardUser(LoyaltyRewardUser loyaltyRewardUser, CancellationToken ct = default)
    {
        await context.LoyaltyRewardUsers.AddAsync(loyaltyRewardUser, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<LoyaltyRewardUser>> GetLoyaltyRewardsByUserId(string userId, CancellationToken ct = default)
    {
        return await context.LoyaltyRewardUsers
            .Where(x => x.UserId == userId)
            .Include(x => x.LoyaltyReward)
            .ToListAsync(ct);
    }
}