namespace HotelManagementApp.Core.Models.HotelModels;

public class HotelReview
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public required string UserId { get; set; }
    public int Rating { get; set; }
    public required string Review { get; set; }
    public DateTime LastModified { get; set; } = DateTime.Now;
    public required HotelModel Hotel { get; set; }

}
