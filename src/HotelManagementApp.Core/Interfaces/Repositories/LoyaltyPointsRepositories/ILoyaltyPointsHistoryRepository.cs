using HotelManagementApp.Core.Models.LoyaltyPointsModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;

public interface ILoyaltyPointsHistoryRepository
{
    Task AddPointsLog(LoyaltyPointsLog log, CancellationToken cancellationToken);
    Task<ICollection<LoyaltyPointsLog>> GetLoyaltyPointsHistoryByGuestId(string guestId, CancellationToken cancellationToken);
    Task<ICollection<LoyaltyPointsLog>> GetAllLoyaltyPointsHistory(CancellationToken cancellationToken);
}
