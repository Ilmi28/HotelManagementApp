namespace HotelManagementApp.Application.Responses.LoyaltyPointsResponses;

public class LoyaltyPointsHistoryLogResponse
{
    public required string GuestId { get; set; }
    public required DateTime Date { get; set; }
    public required int Points { get; set; }
    public required string Description { get; set; }
}