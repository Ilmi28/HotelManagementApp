using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.HotelRepositories;

public class HotelReviewRepository(AppDbContext context) : IHotelReviewRepository
{
    public async Task AddReview(HotelReview review, CancellationToken ct)
    {
        context.Attach(review.Hotel);
        await context.HotelReviews.AddAsync(review, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<HotelReview>> GetAllReviews(CancellationToken ct)
    {
        return await context.HotelReviews
            .AsNoTracking()
            .Include(x => x.ReviewImages)
            .ToListAsync(ct);
    }

    public async Task<ICollection<HotelReview>> GetReviewsByGuest(string guestId, CancellationToken ct)
    {
        return await context.HotelReviews
            .AsNoTracking()
            .Include(x => x.ReviewImages)
            .Where(x => x.UserId == guestId)
            .ToListAsync(ct);
    }

    public async Task<ICollection<HotelReview>> GetReviewsByHotel(int hotelId, CancellationToken ct)
    {
        return await context.HotelReviews
            .AsNoTracking()
            .Include(x => x.ReviewImages)
            .Include(x => x.Hotel)
            .Where(x => x.Hotel.Id == hotelId)
            .ToListAsync(ct);
    }

    public async Task<HotelReview?> GetReviewById(int id, CancellationToken ct)
    {
        return await context.HotelReviews
            .AsNoTracking()
            .Include(x => x.ReviewImages)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task RemoveReview(HotelReview review, CancellationToken ct)
    {
        context.HotelReviews.Remove(review);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateReview(HotelReview review, CancellationToken ct)
    {
        context.HotelReviews.Update(review);
        await context.SaveChangesAsync(ct);
    }
}