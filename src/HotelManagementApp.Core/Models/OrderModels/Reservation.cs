using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Models.OrderModels;

public class Reservation
{
    public int Id { get; set; }
    public required DateOnly From { get; set; }
    public required DateOnly To { get; set; }
    public required HotelRoom Room { get; set; }
    public ICollection<HotelParking> HotelParkings { get; set; } = new List<HotelParking>();
    public ICollection<HotelService> HotelServices { get; set; } = new List<HotelService>();
    public required Order Order { get; set; }
    public decimal Price { get; set; }
}
