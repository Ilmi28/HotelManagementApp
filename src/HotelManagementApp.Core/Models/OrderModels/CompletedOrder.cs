namespace HotelManagementApp.Core.Models.OrderModels;

public class CompletedOrder
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public required int OrderId { get; set; }
    public Order? Order { get; set; }
}
