using HotelManagementApp.Core.Models.RoomModels;

namespace HotelManagementApp.Core.Models.HotelModels;

public class HotelModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required City City { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Description { get; set; }
    public ICollection<RoomModel> Rooms { get; set; } = new List<RoomModel>();
    public ICollection<HotelService> HotelServices { get; set; } = new List<HotelService>();
    public ICollection<HotelImage> HotelImages { get; set; } = new List<HotelImage>();
    public ICollection<HotelParking> HotelParkings { get; set; } = new List<HotelParking>();
}
