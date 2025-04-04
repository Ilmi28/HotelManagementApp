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
    public class TokenRepository : ITokenRepository
    {
        private readonly HotelManagementAppDbContext _context;
        public TokenRepository(HotelManagementAppDbContext context)
        {
            _context = context;
        }

        public async Task AddToken(Token token)
        {
            await _context.Tokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeToken(Token token)
        {
            token.IsRevoked = true;
            _context.Tokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task<Token?> GetLastValidToken(string userId)
        {
            return await _context.Tokens.FirstOrDefaultAsync(x => x.UserId == userId && x.ExpirationDate > DateTime.Now && !x.IsRevoked);
        }

        public async Task<Token?> GetToken(string refreshToken)
        {
            return await _context.Tokens.FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshToken && x.ExpirationDate > DateTime.Now && !x.IsRevoked);
        }
    }
}
