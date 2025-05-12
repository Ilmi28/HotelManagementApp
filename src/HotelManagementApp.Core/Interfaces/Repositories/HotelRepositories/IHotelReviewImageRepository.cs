using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;

public interface IHotelReviewImageRepository
{
    Task AddReviewImage(HotelReviewImage reviewImage, CancellationToken ct);
    Task<ICollection<HotelReviewImage>> GetReviewImagesByReviewId(int reviewId, CancellationToken ct);
    Task RemoveReviewImagesByReviewId(int reviewId, CancellationToken ct);
}