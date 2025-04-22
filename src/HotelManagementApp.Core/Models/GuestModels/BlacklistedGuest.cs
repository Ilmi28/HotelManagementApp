namespace HotelManagementApp.Core.Models.GuestModels;

public class BlacklistedGuest
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public DateTime BlacklistedDate { get; set; } = DateTime.Now;
}
