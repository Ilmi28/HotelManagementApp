using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class BlacklistUserNotFound(string message) : NotFoundException(message)
{

}
