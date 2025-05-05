using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Core.Models.OrderModels;

public class OrderDetails
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}
