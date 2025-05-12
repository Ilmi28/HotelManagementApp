namespace HotelManagementApp.Core.Models.PaymentModels;

public class CashPayment
{
    public int Id { get; set; }
    public required Payment Payment { get; set; }
}