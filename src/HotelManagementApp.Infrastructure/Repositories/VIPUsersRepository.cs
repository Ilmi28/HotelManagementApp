using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Infrastructure.Repositories
{
    public class VIPUsersRepository
    {
        public Task<bool> AddUserToVIP(string userId)
        {
            throw new NotImplementedException();
        }
        public Task<List<string>> GetVIPUsers()
        {
            throw new NotImplementedException();
        }
        public Task<bool> IsUserVIP(string userId)
        {
            throw new NotImplementedException();
        }
        public Task<bool> RemoveUserFromVIP(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
