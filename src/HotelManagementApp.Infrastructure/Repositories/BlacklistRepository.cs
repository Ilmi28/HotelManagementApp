using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class BlacklistRepository(AppDbContext context) : IBlacklistRepository
{
    public async Task AddUserToBlacklist(string userId, CancellationToken ct)
    {
        await context.BlackListedGuests.AddAsync(new BlacklistedGuest
        {
            UserId = userId
        }, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<List<BlacklistedGuest>> GetBlackList(CancellationToken ct)
    {
        return await context.BlackListedGuests.ToListAsync(ct);
    }

    public async Task<bool> IsUserBlacklisted(string userId, CancellationToken ct)
    {
        var user = await context.BlackListedGuests.FirstOrDefaultAsync(x => x.UserId == userId, ct);
        if (user == null)
            return true;
        return false;
    }

    public async Task RemoveUserFromBlacklist(string userId, CancellationToken ct)
    {
        var user = await context.BlackListedGuests.FirstOrDefaultAsync(x => x.UserId == userId, ct);
        if (user != null)
        {
            context.BlackListedGuests.Remove(user);
            await context.SaveChangesAsync(ct);
        }
    }
}
