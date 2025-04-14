namespace HotelManagementApp.Core.Models;

public class VIPGuest
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public DateTime VIPDate { get; set; } = DateTime.Now;
}
