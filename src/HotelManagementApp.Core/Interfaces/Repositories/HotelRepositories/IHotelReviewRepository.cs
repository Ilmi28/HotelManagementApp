using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;

public interface IHotelReviewRepository
{
    Task AddReview(HotelReview review, CancellationToken ct);
    Task<ICollection<HotelReview>> GetAllReviews(CancellationToken ct);
    Task<ICollection<HotelReview>> GetReviewsByGuest(string guestId, CancellationToken ct);
    Task<ICollection<HotelReview>> GetReviewsByHotel(int hotelId, CancellationToken ct);
    Task<HotelReview?> GetReviewById(int id, CancellationToken ct);
    Task RemoveReview(HotelReview review, CancellationToken ct);
    Task UpdateReview(HotelReview review, CancellationToken ct);
}