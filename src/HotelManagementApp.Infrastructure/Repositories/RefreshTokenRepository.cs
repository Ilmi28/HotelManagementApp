using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.TokenModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
{
    public async Task AddToken(RefreshToken token, CancellationToken ct)
    {
        await context.RefreshTokens.AddAsync(token, ct);
        await context.SaveChangesAsync(ct);
    }
    public async Task DeleteToken(RefreshToken token, CancellationToken ct)
    {
        context.RefreshTokens.Remove(token);
        await context.SaveChangesAsync(ct);
    }

    public async Task<RefreshToken?> GetTokenByUser(string userId, CancellationToken ct)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId, ct);
    }

    public async Task<RefreshToken?> GetToken(string refreshToken, CancellationToken ct)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshToken, ct);
    }
}
