using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class PaymentNotFoundException(string message) : NotFoundException(message)
{
    
}