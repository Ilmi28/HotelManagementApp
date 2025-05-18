using HotelManagementApp.Core.Interfaces.Services;

namespace HotelManagementApp.Infrastructure.Services;

public class FakeCreditCardPaymentService() : ICreditCardPaymentService
{
    public async Task Pay(string number, string cvv, string expirationDate)
    {
        await Task.Delay(1000);
        Console.WriteLine($"Credit card number: {number}");
        Console.WriteLine($"Credit card cvv: {cvv}");
        Console.WriteLine($"Credit card expiration date: {expirationDate}");
        Console.WriteLine("Credit card payment successful");
    }
}