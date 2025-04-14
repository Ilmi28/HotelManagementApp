using HotelManagementApp.Core.Models;

namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface IBlacklistRepository
{
    Task AddUserToBlacklist(string userId);
    Task RemoveUserFromBlacklist(string userId);
    Task<bool> IsUserBlacklisted(string userId);
    Task<List<BlacklistedGuest>> GetBlackList();
}
