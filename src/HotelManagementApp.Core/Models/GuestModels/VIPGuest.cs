namespace HotelManagementApp.Core.Models.GuestModels;

public class VIPGuest
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public DateTime VIPDate { get; set; } = DateTime.Now;
}
