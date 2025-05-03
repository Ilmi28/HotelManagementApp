using HotelManagementApp.Core.Enums;

namespace HotelManagementApp.Core.Models.HotelModels;

public class HotelRoom
{
    public int Id { get; set; }
    public required string RoomName { get; set; }
    public required RoomTypeEnum RoomType { get; set; }
    public required decimal Price { get; set; }
    public required string Description { get; set; }
    public required Hotel Hotel { get; set; }
    public ICollection<HotelRoomImage> RoomImages { get; set; } = new List<HotelRoomImage>();
}
