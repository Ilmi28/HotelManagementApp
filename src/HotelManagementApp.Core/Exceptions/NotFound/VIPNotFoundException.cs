using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class VIPNotFoundException(string message) : NotFoundException(message)
{

}
