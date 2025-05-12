namespace HotelManagementApp.Core.Models.HotelModels;

public class HotelReview
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public required string UserId { get; set; }
    public int Rating { get; set; }
    public required string? Review { get; set; }
    public required DateTime Created { get; set; }
    public DateTime LastModified { get; set; } = DateTime.Now;
    public required Hotel Hotel { get; set; }
    public ICollection<HotelReviewImage> ReviewImages { get; set; } = new List<HotelReviewImage>();
}
