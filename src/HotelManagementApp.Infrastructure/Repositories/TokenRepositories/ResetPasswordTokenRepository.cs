using HotelManagementApp.Core.Interfaces.Repositories.TokenRepositories;
using HotelManagementApp.Core.Models.TokenModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.TokenRepositories;

public class ResetPasswordTokenRepository(AppDbContext context) : IResetPasswordTokenRepository
{
    public async Task AddToken(ResetPasswordToken token, CancellationToken ct)
    {
        await context.ResetPasswordTokens.AddAsync(token, ct);
        await context.SaveChangesAsync(ct);
    }
    public async Task DeleteToken(ResetPasswordToken token, CancellationToken ct)
    {
        context.ResetPasswordTokens.Remove(token);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ResetPasswordToken?> GetTokenByUser(string userId, CancellationToken ct)
    {
        return await context.ResetPasswordTokens.FirstOrDefaultAsync(x => x.UserId == userId, ct);
    }

    public async Task<ResetPasswordToken?> GetToken(string token, CancellationToken ct)
    {
        return await context.ResetPasswordTokens.FirstOrDefaultAsync(x => x.ResetPasswordTokenHash == token, ct);
    }
}
