using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class HotelNotFoundException(string message) :NotFoundException(message)
{

}
