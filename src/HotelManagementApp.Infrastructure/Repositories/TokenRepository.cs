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

        public async Task<bool> AddToken(Token token)
        {
            var user = await _context.Users.FindAsync(token.UserId);
            if (user == null)
                return false;
            var lastToken = _context.Tokens.FirstOrDefault(x => x.UserId == token.UserId && !x.IsRevoked);
            if (lastToken != null)
                lastToken.IsRevoked = true;
            await _context.Tokens.AddAsync(token);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Token?> GetToken(string refreshToken)
        {
            return await _context.Tokens.FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshToken && x.ExpirationDate > DateTime.Now && !x.IsRevoked);
        }
    }
}
