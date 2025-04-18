namespace HotelManagementApp.Core.Models;

public class Room
{
    public int Id { get; set; }
    public required string RoomNumber { get; set; }
    public required string RoomType { get; set; }
    public required decimal Price { get; set; }
    public required string Description { get; set; }
    public required Hotel Hotel { get; set; }
}
