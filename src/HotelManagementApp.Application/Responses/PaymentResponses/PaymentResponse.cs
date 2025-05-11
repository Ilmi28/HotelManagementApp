using HotelManagementApp.Core.Enums;

namespace HotelManagementApp.Application.Responses.OrderResponses;

public class PaymentResponse
{
    public required int Id { get; set; }
    public required int OrderId { get; set; }
    public required string PaymentMethod { get; set; }
    public required decimal Amount { get; set; }
    public DateTime Date { get; set; }
}