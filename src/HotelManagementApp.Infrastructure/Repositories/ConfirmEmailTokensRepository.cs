using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.TokenModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class ConfirmEmailTokensRepository(AppDbContext context) : IConfirmEmailTokensRepository
{
    public async Task AddToken(ConfirmEmailToken token, CancellationToken ct)
    {
        await context.ConfirmEmailTokens.AddAsync(token, ct);
        await context.SaveChangesAsync(ct);
    }
    public async Task DeleteToken(ConfirmEmailToken token, CancellationToken ct)
    {
        context.ConfirmEmailTokens.Remove(token);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ConfirmEmailToken?> GetTokenByUser(string userId, CancellationToken ct)
    {
        return await context.ConfirmEmailTokens.FirstOrDefaultAsync(x => x.UserId == userId, ct);
    }

    public async Task<ConfirmEmailToken?> GetToken(string token, CancellationToken ct)
    {
        return await context.ConfirmEmailTokens.FirstOrDefaultAsync(x => x.ConfirmEmailTokenHash == token, ct);
    }
}
