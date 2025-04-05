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
    public class VIPUserRepository : IVIPUserRepository
    {
        private readonly HotelManagementAppDbContext _context;
        public VIPUserRepository(HotelManagementAppDbContext context)
        {
            _context = context;
        }
        public async Task AddUserToVIP(string userId)
        {
            await _context.VIPUsers.AddAsync(new VIPUser
            {
                UserId = userId
            });
            await _context.SaveChangesAsync();
        }
        public async Task<List<VIPUser>> GetVIPUsers()
        {
            return await _context.VIPUsers.ToListAsync();
        }
        public async Task<bool> IsUserVIP(string userId)
        {
            var user = await _context.VIPUsers.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null)
                return false;
            return true;
        }
        public async Task RemoveUserFromVIP(string userId)
        {
            var user = _context.VIPUsers.FirstOrDefault(x => x.UserId == userId);
            if (user != null)
            {
                _context.VIPUsers.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
