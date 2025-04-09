namespace HotelManagementApp.Core.Models;

public class VIPUser
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public DateTime VIPDate { get; set; } = DateTime.Now;
}
