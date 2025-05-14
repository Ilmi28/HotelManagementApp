using HotelManagementApp.Core.Models.LoyaltyPointsModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.LoyaltyPointsRepositories;

public interface ILoyaltyPointsRepository
{
    Task AddLoyaltyPoints(LoyaltyPoints loyaltyPoints, CancellationToken cancellationToken);
    Task UpdateLoyaltyPoints(LoyaltyPoints loyaltyPoints, CancellationToken cancellationToken);
    Task<LoyaltyPoints?> GetLoyaltyPointsByGuestId(string id, CancellationToken cancellationToken);
    Task<ICollection<LoyaltyPoints>> GetAllLoyaltyPoints(CancellationToken cancellationToken);
}
