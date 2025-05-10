namespace HotelManagementApp.Application.Responses.OrderResponses;

public class OrderResponse
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public required string Status { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Confirmed { get; set; }
    public DateTime? Completed { get; set; }
    public DateTime? Canceled { get; set; }
    
}