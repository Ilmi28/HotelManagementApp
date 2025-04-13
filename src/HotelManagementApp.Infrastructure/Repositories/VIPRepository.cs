using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class VIPRepository(AppDbContext context) : IVIPRepository
{
    public async Task AddUserToVIP(string userId)
    {
        await context.VIPGuests.AddAsync(new VIPGuest
        {
            UserId = userId
        });
        await context.SaveChangesAsync();
    }
    public async Task<List<VIPGuest>> GetVIPUsers()
    {
        return await context.VIPGuests.ToListAsync();
    }
    public async Task<bool> IsUserVIP(string userId)
    {
        var user = await context.VIPGuests.FirstOrDefaultAsync(x => x.UserId == userId);
        if (user == null)
            return false;
        return true;
    }
    public async Task RemoveUserFromVIP(string userId)
    {
        var user = context.VIPGuests.FirstOrDefault(x => x.UserId == userId);
        if (user != null)
        {
            context.VIPGuests.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
