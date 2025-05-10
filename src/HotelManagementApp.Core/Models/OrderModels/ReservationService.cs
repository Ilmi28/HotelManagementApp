using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Models.OrderModels;

public class ReservationService
{
    public int Id { get; set; }
    public required Reservation Reservation { get; set; }
    public required HotelService HotelService { get; set; }
}