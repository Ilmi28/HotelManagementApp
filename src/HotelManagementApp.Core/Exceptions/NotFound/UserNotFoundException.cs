using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class UserNotFoundException(string message) : NotFoundException(message)
{
}
