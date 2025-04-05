using HotelManagementApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Interfaces.Repositories
{
    public interface IBlacklistedUserRepository
    {
        Task AddUserToBlacklist(string userId);
        Task RemoveUserFromBlacklist(string userId);
        Task<bool> IsUserBlacklisted(string userId);
        Task<List<BlacklistedUser>> GetBlackList();
    }
}
