using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Models.OrderModels;

public class ReservationParking
{
    public int Id { get; set; }
    public required Reservation Reservation { get; set; }
    public required HotelParking HotelParking { get; set; }
}