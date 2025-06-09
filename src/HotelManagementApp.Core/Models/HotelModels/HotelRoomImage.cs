namespace HotelManagementApp.Core.Models.HotelModels;

public class HotelRoomImage
{
    public int Id { get; set; }
    public required string FileName { get; set; }
    public required HotelRoom Room { get; set; }
    public int RoomId { get; set; }
}
