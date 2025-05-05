using HotelManagementApp.Core.Dtos;

namespace HotelManagementApp.Core.Interfaces.Identity;

public interface IUserManager
{
    Task<bool> CreateAsync(UserDto user, string password);
    Task<UserDto?> FindByIdAsync(string userId);
    Task<UserDto?> FindByEmailAsync(string email);
    Task<UserDto?> FindByNameAsync(string userName);
    Task<bool> CheckPasswordAsync(UserDto user, string password);
    Task<bool> DeleteAsync(UserDto user);
    Task<bool> UpdateAsync(UserDto user);
    Task<bool> ChangePasswordAsync(UserDto user, string currentPassword, string newPassword);
    Task<ICollection<UserDto>> GetUsersWithoutRole();
    Task<bool> ResetPasswordAsync(UserDto user, string newPassword);
}
