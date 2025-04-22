using HotelManagementApp.Core.Models.GuestModels;

namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface IBlacklistRepository
{
    Task AddUserToBlacklist(string userId, CancellationToken ct = default);
    Task RemoveUserFromBlacklist(string userId, CancellationToken ct = default);
    Task<bool> IsUserBlacklisted(string userId, CancellationToken ct = default);
    Task<List<BlacklistedGuest>> GetBlackList(CancellationToken ct = default);
}
