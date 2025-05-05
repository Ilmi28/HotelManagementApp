using HotelManagementApp.Core.Models.LoyaltyPointsModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;

public interface ILoyaltyPointsHistoryRepository
{
    Task AddPointsLog(LoyaltyPointsLog log);
    Task<ICollection<LoyaltyPointsLog>> GetLoyaltyPointsHistoryByGuestId(string guestId);
}
