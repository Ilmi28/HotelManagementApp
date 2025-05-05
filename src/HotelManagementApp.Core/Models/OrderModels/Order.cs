using HotelManagementApp.Core.Enums;

namespace HotelManagementApp.Core.Models.OrderModels;

public class Order
{
    public int Id { get; set; }
    public OrderDetails OrderDetails { get; set; } = null!;
    public required OrderStatusEnum Status { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public required string UserId { get; set; }
    public decimal TotalPrice { get; set; }

}
