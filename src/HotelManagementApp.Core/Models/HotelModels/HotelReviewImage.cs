namespace HotelManagementApp.Core.Models.HotelModels;

public class HotelReviewImage
{
    public int Id { get; set; }
    public required string FileName { get; set; }
    public required HotelReview HotelReview { get; set; }
}