using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Infrastructure.Repositories
{
    public class BlacklistedUsersRepository : IBlacklistedUsersRepository
    {
        private HotelManagementAppDbContext _context;
        public BlacklistedUsersRepository(HotelManagementAppDbContext context)
        {
            _context = context;
        }
        public async Task AddUserToBlacklist(string userId)
        {
            await _context.BlackListedUsers.AddAsync(new BlacklistedUsers
            {
                UserId = userId
            });
            await _context.SaveChangesAsync();
        }

        public Task<List<BlacklistedUsers>> GetBlackList()
        {
            throw new NotImplementedException();
        }

        public Task IsUserBlacklisted(string userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserFromBlacklist(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
