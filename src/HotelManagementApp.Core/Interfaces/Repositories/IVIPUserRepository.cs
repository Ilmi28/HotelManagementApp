using HotelManagementApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Core.Interfaces.Repositories
{
    public interface IVIPUserRepository
    {
        Task AddUserToVIP(string userId);
        Task RemoveUserFromVIP(string userId);
        Task<bool> IsUserVIP(string userId);
        Task<List<VIPUser>> GetVIPUsers();
    }
}
