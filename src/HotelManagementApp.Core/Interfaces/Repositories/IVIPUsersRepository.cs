using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Interfaces.Repositories
{
    public interface IVIPUsersRepository
    {
        Task<bool> AddUserToVIP(string userId);
        Task<bool> RemoveUserFromVIP(string userId);
        Task<bool> IsUserVIP(string userId);
        Task<List<string>> GetVIPUsers();
    }
}
