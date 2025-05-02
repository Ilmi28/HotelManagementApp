using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class HotelServiceNotFoundException(string message) : NotFoundException(message)
{

}
