namespace HotelManagementApp.Application.Responses.HotelResponses;

public class HotelReviewResponse
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public int Rating { get; set; }
    public required string? Review { get; set; }
    public required DateTime Created { get; set; }
    public required DateTime LastModified { get; set; }
    public required int HotelId { get; set; }
    public ICollection<string> ReviewImages { get; set; } = new List<string>();
}