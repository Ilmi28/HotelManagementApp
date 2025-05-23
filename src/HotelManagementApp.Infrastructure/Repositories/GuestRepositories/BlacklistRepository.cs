﻿using HotelManagementApp.Core.Interfaces.Repositories.GuestRepositories;
using HotelManagementApp.Core.Models.GuestModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.GuestRepositories;

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
        return user != null;
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
