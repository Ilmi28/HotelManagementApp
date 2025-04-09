using HotelManagementApp.Core.Models;

namespace HotelManagementApp.Core.Interfaces.Repositories;

public interface IVIPUserRepository
{
    Task AddUserToVIP(string userId);
    Task RemoveUserFromVIP(string userId);
    Task<bool> IsUserVIP(string userId);
    Task<List<VIPUser>> GetVIPUsers();
}
