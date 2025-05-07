using HotelManagementApp.Core.Interfaces.Repositories.GuestRepositories;
using HotelManagementApp.Core.Models.GuestModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.GuestRepositories;

public class VIPRepository(AppDbContext context) : IVIPRepository
{
    public async Task AddUserToVIP(string userId, CancellationToken ct)
    {
        await context.VipGuests.AddAsync(new VIPGuest
        {
            UserId = userId
        }, ct);
        await context.SaveChangesAsync(ct);
    }
    public async Task<List<VIPGuest>> GetVIPUsers(CancellationToken ct)
    {
        return await context.VipGuests.ToListAsync(ct);
    }
    public async Task<bool> IsUserVIP(string userId, CancellationToken ct)
    {
        var user = await context.VipGuests.FirstOrDefaultAsync(x => x.UserId == userId, ct);
        if (user == null)
            return false;
        return true;
    }
    public async Task RemoveUserFromVIP(string userId, CancellationToken ct)
    {
        var user = await context.VipGuests.FirstOrDefaultAsync(x => x.UserId == userId, ct);
        if (user != null)
        {
            context.VipGuests.Remove(user);
            await context.SaveChangesAsync(ct);
        }
    }
}
