using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class RefreshTokenRepository(AppDbContext context) : ITokenRepository
{
    public async Task AddToken(RefreshToken token, CancellationToken ct)
    {
        await context.RefreshTokens.AddAsync(token, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task RevokeToken(RefreshToken token, CancellationToken ct)
    {
        token.IsRevoked = true;
        context.RefreshTokens.Update(token);
        await context.SaveChangesAsync(ct);
    }

    public async Task<RefreshToken?> GetLastValidToken(string userId, CancellationToken ct)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(
            x => x.UserId == userId && x.ExpirationDate > DateTime.Now && !x.IsRevoked, ct);
    }

    public async Task<RefreshToken?> GetToken(string refreshToken, CancellationToken ct)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshToken, ct);
    }
}
