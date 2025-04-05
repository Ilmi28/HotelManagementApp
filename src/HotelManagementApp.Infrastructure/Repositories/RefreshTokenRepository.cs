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
    public class RefreshTokenRepository : ITokenRepository
    {
        private readonly HotelManagementAppDbContext _context;
        public RefreshTokenRepository(HotelManagementAppDbContext context)
        {
            _context = context;
        }

        public async Task AddToken(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeToken(RefreshToken token)
        {
            token.IsRevoked = true;
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetLastValidToken(string userId)
        {
            return await _context.RefreshTokens.LastOrDefaultAsync(x => x.UserId == userId && x.ExpirationDate > DateTime.Now && !x.IsRevoked);
        }

        public async Task<RefreshToken?> GetToken(string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshToken && x.ExpirationDate > DateTime.Now && !x.IsRevoked);
        }
    }
}
