using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.HotelRepositories;

public class HotelReviewImageRepository(AppDbContext context) : IHotelReviewImageRepository
{
    public async Task AddReviewImage(HotelReviewImage reviewImage, CancellationToken ct)
    {
        context.Attach(reviewImage.HotelReview);
        await context.HotelReviewImages.AddAsync(reviewImage, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<HotelReviewImage>> GetReviewImagesByReviewId(int reviewId, CancellationToken ct)
    {
        return await context.HotelReviewImages
            .AsNoTracking()
            .Include(x => x.HotelReview)
            .Where(x => x.HotelReview.Id == reviewId)
            .ToListAsync(ct);
    }

    public async Task RemoveReviewImagesByReviewId(int reviewId, CancellationToken ct)
    {
        var reviewImages = await context.HotelReviewImages
            .Where(x => x.HotelReviewId == reviewId).ToListAsync(ct);
        foreach (var reviewImage in reviewImages)
        {
            context.HotelReviewImages.Remove(reviewImage);
        }
        await context.SaveChangesAsync(ct);
    }
}