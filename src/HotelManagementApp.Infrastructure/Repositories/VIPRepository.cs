using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class VIPRepository(AppDbContext context) : IVIPRepository
{
    public async Task AddUserToVIP(string userId, CancellationToken ct)
    {
        await context.VIPGuests.AddAsync(new VIPGuest
        {
            UserId = userId
        }, ct);
        await context.SaveChangesAsync(ct);
    }
    public async Task<List<VIPGuest>> GetVIPUsers(CancellationToken ct)
    {
        return await context.VIPGuests.ToListAsync(ct);
    }
    public async Task<bool> IsUserVIP(string userId, CancellationToken ct)
    {
        var user = await context.VIPGuests.FirstOrDefaultAsync(x => x.UserId == userId, ct);
        if (user == null)
            return false;
        return true;
    }
    public async Task RemoveUserFromVIP(string userId, CancellationToken ct)
    {
        var user = await context.VIPGuests.FirstOrDefaultAsync(x => x.UserId == userId, ct);
        if (user != null)
        {
            context.VIPGuests.Remove(user);
            await context.SaveChangesAsync(ct);
        }
    }
}
