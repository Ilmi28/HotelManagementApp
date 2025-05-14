using HotelManagementApp.Core.Models.LoyaltyPointsModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;

public interface ILoyaltyRewardUserRepository
{
    Task AddLoyaltyRewardUser(LoyaltyRewardUser loyaltyRewardUser, CancellationToken ct = default);
    Task<ICollection<LoyaltyRewardUser>> GetLoyaltyRewardsByUserId(string userId, CancellationToken ct = default);
}