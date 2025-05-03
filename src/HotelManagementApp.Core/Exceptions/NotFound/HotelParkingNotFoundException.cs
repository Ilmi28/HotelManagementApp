using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class HotelParkingNotFoundException(string message) : NotFoundException(message)
{

}
