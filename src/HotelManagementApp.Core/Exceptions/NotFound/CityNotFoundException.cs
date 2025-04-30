using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class CityNotFoundException(string message) : NotFoundException(message)
{
}
