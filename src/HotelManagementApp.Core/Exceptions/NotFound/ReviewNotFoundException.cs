using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class ReviewNotFoundException(string message) : NotFoundException(message)
{
    
}