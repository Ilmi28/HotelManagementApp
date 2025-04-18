using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.BadRequest;

public class InvalidImageTypeException(string message) : BadRequestException(message)
{

}
