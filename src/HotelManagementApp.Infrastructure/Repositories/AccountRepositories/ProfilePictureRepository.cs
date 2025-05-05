using HotelManagementApp.Core.Interfaces.Repositories.AccountRepositories;
using HotelManagementApp.Core.Models.AccountModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.AccountRepositories;

public class ProfilePictureRepository(AppDbContext context) : IProfilePictureRepository
{
    public async Task<ProfilePicture?> GetProfilePicture(string userId, CancellationToken ct)
    {
        return await context.ProfilePictures
            .FirstOrDefaultAsync(x => x.UserId == userId, ct);
    }

    public async Task AddProfilePicture(ProfilePicture profilePicture, CancellationToken ct)
    {
        await context.ProfilePictures.AddAsync(profilePicture, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task RemoveProfilePicture(string userId, CancellationToken ct)
    {
        var profilePicture = await context.ProfilePictures
            .FirstOrDefaultAsync(x => x.UserId == userId, ct);
        if (profilePicture != null)
        {
            context.ProfilePictures.Remove(profilePicture);
            await context.SaveChangesAsync(ct);
        }
    }
}
