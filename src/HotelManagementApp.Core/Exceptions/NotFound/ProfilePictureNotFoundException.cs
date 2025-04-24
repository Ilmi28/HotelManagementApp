using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class ProfilePictureNotFoundException(string message) : NotFoundException(message)
{

}
