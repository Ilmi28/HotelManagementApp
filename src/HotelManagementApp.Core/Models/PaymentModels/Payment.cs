using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Models.PaymentModels;

public class Payment
{
    public int Id { get; set; }
    public required PaymentMethodEnum PaymentMethod { get; set; }
    public required Order Order { get; set; }
    public required decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}
