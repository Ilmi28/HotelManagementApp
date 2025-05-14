using HotelManagementApp.Core.Models.LoyaltyPointsModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;

public interface ILoyaltyRewardsRepository
{
    Task<LoyaltyReward?> GetLoyaltyRewardById(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoyaltyReward>> GetAllLoyaltyRewards(CancellationToken cancellationToken = default);
    Task AddLoyaltyReward(LoyaltyReward loyaltyReward, CancellationToken cancellationToken = default);
    Task UpdateLoyaltyReward(LoyaltyReward loyaltyReward, CancellationToken cancellationToken = default);
    Task RemoveLoyaltyReward(LoyaltyReward loyaltyReward, CancellationToken cancellationToken = default);
}