using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class BlacklistRepository(AppDbContext context) : IBlacklistRepository
{
    public async Task AddUserToBlacklist(string userId)
    {
        await context.BlackListedGuests.AddAsync(new BlacklistedGuest
        {
            UserId = userId
        });
        await context.SaveChangesAsync();
    }

    public async Task<List<BlacklistedGuest>> GetBlackList()
    {
        return await context.BlackListedGuests.ToListAsync();
    }

    public async Task<bool> IsUserBlacklisted(string userId)
    {
        var user = await context.BlackListedGuests.FirstOrDefaultAsync(x => x.UserId == userId);
        if (user == null)
            return true;
        return false;
    }

    public async Task RemoveUserFromBlacklist(string userId)
    {
        var user = context.BlackListedGuests.FirstOrDefault(x => x.UserId == userId);
        if (user != null)
        {
            context.BlackListedGuests.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
