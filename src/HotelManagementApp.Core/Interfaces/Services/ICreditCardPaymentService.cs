namespace HotelManagementApp.Core.Interfaces.Services;

public interface ICreditCardPaymentService
{
    Task Pay(string number, string cvv, string expirationDate);
}