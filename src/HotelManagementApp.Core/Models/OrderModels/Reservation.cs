using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Models.OrderModels;

public class Reservation
{
    public int Id { get; set; }
    public required DateOnly From { get; set; }
    public required DateOnly To { get; set; }
    public required HotelRoom Room { get; set; }
    public ICollection<ReservationParking> ReservationParkings { get; set; } = new List<ReservationParking>();
    public ICollection<ReservationService> ReservationServices { get; set; } = new List<ReservationService>();
    public required Order Order { get; set; }
}
