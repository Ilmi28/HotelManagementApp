namespace HotelManagementApp.Core.Models.LoyaltyPointsModels;

public class LoyaltyPoints
{
    public int Id { get; set; }
    public required string GuestId { get; set; }
    public int Points { get; set; }
}
