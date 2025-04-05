using HotelManagementApp.Core.Dtos;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Infrastructure.Repositories
{
    public class BlacklistedUserRepository : IBlacklistedUserRepository
    {
        private HotelManagementAppDbContext _context;
        public BlacklistedUserRepository(HotelManagementAppDbContext context)
        {
            _context = context;
        }
        public async Task AddUserToBlacklist(string userId)
        {
            await _context.BlackListedUsers.AddAsync(new BlacklistedUser
            {
                UserId = userId
            });
            await _context.SaveChangesAsync();
        }

        public async Task<List<BlacklistedUser>> GetBlackList()
        {
            return await _context.BlackListedUsers.ToListAsync();
        }

        public async Task<bool> IsUserBlacklisted(string userId)
        {
            var user = await _context.BlackListedUsers.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null)
                return true;
            return false;
        }

        public async Task RemoveUserFromBlacklist(string userId)
        {
           var user = _context.BlackListedUsers.FirstOrDefault(x => x.UserId == userId);
            if (user != null)
            {
                _context.BlackListedUsers.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
