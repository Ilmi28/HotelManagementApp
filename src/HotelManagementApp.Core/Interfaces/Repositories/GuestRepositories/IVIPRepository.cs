using HotelManagementApp.Core.Models.GuestModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.GuestRepositories;

public interface IVIPRepository
{
    Task AddUserToVIP(string userId, CancellationToken ct = default);
    Task RemoveUserFromVIP(string userId, CancellationToken ct = default);
    Task<bool> IsUserVIP(string userId, CancellationToken ct = default);
    Task<List<VIPGuest>> GetVIPUsers(CancellationToken ct = default);
}
