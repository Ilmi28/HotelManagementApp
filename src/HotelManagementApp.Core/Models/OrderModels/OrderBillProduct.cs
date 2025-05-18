namespace HotelManagementApp.Core.Models.OrderModels;

public class OrderBillProduct
{
    public int Id { get; set; }
    public required int OrderId { get; set; }
    public Order? Order { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public int Quantity { get; set; } = 1;
}