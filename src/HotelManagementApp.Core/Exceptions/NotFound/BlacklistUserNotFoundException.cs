using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class BlacklistUserNotFoundException(string message) : NotFoundException(message)
{

}
