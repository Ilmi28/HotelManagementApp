using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Models.RoomModels;

public class RoomModel
{
    public int Id { get; set; }
    public required string RoomName { get; set; }
    public required RoomTypeEnum RoomType { get; set; }
    public required decimal Price { get; set; }
    public required string Description { get; set; }
    public required HotelModel Hotel { get; set; }
    public ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>();
}
