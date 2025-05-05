namespace HotelManagementApp.Core.Models.OrderModels;

public class CancelledOrder
{
    public int Id { get; set; }
    public required DateTime Date { get; set; } = DateTime.Now;
    public Order Order { get; set; } = null!;
}
