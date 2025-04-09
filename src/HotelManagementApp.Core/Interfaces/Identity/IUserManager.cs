using HotelManagementApp.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Interfaces.Identity
{
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
        Task<ICollection<UserDto>> GetUsersInRoleAsync(string role);
        Task<bool> IsUserInRoleAsync(string userId, string roleName);
        Task<IList<string>> GetUserRolesAsync(string userId);
    }
}
