namespace HotelManagementApp.Core.Models.RoomModels;

public class RoomImage
{
    public int Id { get; set; }
    public required string FileName { get; set; }
    public required RoomModel Room { get; set; }
}
