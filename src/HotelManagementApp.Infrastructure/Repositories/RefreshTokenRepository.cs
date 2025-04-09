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
    public class RefreshTokenRepository(HotelManagementAppDbContext context) : ITokenRepository
    {
        public async Task AddToken(RefreshToken token)
        {
            await context.RefreshTokens.AddAsync(token);
            await context.SaveChangesAsync();
        }

        public async Task RevokeToken(RefreshToken token)
        {
            token.IsRevoked = true;
            context.RefreshTokens.Update(token);
            await context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetLastValidToken(string userId)
        {
            return await context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.ExpirationDate > DateTime.Now && !x.IsRevoked);
        }

        public async Task<RefreshToken?> GetToken(string refreshToken)
        {
            return await context.RefreshTokens.FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshToken && x.ExpirationDate > DateTime.Now && !x.IsRevoked);
        }
    }
}
