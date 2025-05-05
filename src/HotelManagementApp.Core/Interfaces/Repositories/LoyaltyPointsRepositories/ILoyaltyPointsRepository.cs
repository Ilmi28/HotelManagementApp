using HotelManagementApp.Core.Models.LoyaltyPointsModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;

public interface ILoyaltyPointsRepository
{
    Task AddLoyaltyPoints(LoyaltyPoints loyaltyPoints);
    Task UpdateLoyaltyPoints(LoyaltyPoints loyaltyPoints);
    Task<LoyaltyPoints?> GetLoyaltyPointsByGuestId(string id);
    Task<ICollection<LoyaltyReward>> GetLoyaltyRewards();
}
