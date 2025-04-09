using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class BlacklistedUserRepository(HotelManagementAppDbContext context) : IBlacklistedUserRepository
{
    public async Task AddUserToBlacklist(string userId)
    {
        await context.BlackListedUsers.AddAsync(new BlacklistedUser
        {
            UserId = userId
        });
        await context.SaveChangesAsync();
    }

    public async Task<List<BlacklistedUser>> GetBlackList()
    {
        return await context.BlackListedUsers.ToListAsync();
    }

    public async Task<bool> IsUserBlacklisted(string userId)
    {
        var user = await context.BlackListedUsers.FirstOrDefaultAsync(x => x.UserId == userId);
        if (user == null)
            return true;
        return false;
    }

    public async Task RemoveUserFromBlacklist(string userId)
    {
        var user = context.BlackListedUsers.FirstOrDefault(x => x.UserId == userId);
        if (user != null)
        {
            context.BlackListedUsers.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
