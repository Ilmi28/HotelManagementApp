using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class RoomNotFoundException(string message) : NotFoundException(message)
{

}
