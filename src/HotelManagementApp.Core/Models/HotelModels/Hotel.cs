namespace HotelManagementApp.Core.Models.HotelModels;

public class Hotel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required City City { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Description { get; set; }
    public ICollection<HotelRoom> Rooms { get; set; } = new List<HotelRoom>();
    public ICollection<HotelService> HotelServices { get; set; } = new List<HotelService>();
    public ICollection<HotelImage> HotelImages { get; set; } = new List<HotelImage>();
    public ICollection<HotelParking> HotelParkings { get; set; } = new List<HotelParking>();
}
