using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories;

public class VIPUserRepository(HotelManagementAppDbContext context) : IVIPUserRepository
{
    public async Task AddUserToVIP(string userId)
    {
        await context.VIPUsers.AddAsync(new VIPUser
        {
            UserId = userId
        });
        await context.SaveChangesAsync();
    }
    public async Task<List<VIPUser>> GetVIPUsers()
    {
        return await context.VIPUsers.ToListAsync();
    }
    public async Task<bool> IsUserVIP(string userId)
    {
        var user = await context.VIPUsers.FirstOrDefaultAsync(x => x.UserId == userId);
        if (user == null)
            return false;
        return true;
    }
    public async Task RemoveUserFromVIP(string userId)
    {
        var user = context.VIPUsers.FirstOrDefault(x => x.UserId == userId);
        if (user != null)
        {
            context.VIPUsers.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
