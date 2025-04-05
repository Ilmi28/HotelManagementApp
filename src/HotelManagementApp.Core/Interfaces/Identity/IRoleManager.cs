using HotelManagementApp.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Interfaces.Identity
{
    public interface IRoleManager
    {
        Task<bool> RoleExistsAsync(string roleName);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<bool> IsUserInRoleAsync(string userId, string roleName);
        Task<List<string>> GetAllRolesAsync();
        Task<ICollection<UserDto>> GetUsersInRoleAsync(string roleName);
    }
}
