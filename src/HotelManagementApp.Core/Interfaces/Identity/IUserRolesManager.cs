using HotelManagementApp.Core.Dtos;

namespace HotelManagementApp.Core.Interfaces.Identity;

public interface IUserRolesManager
{
    Task<ICollection<UserDto>> GetUsersInRoleAsync(string role);
    Task<bool> IsUserInRoleAsync(string userId, string roleName);
    Task<ICollection<string>> GetUserRolesAsync(string userId);
    Task<bool> AddToRoleAsync(UserDto user, string role);
    Task<bool> RemoveFromRoleAsync(UserDto user, string role);
}
