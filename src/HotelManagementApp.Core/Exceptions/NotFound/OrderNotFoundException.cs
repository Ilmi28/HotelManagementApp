using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class OrderNotFoundException(string message) : NotFoundException(message)
{
    
}