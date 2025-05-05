using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.CreateOrder;

public class CreateOrderCommand : IRequest
{
    public required string UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
}
