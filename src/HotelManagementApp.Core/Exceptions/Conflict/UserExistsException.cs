using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.Conflict;

public class UserExistsException(string message) : ConflictException(message)
{
}
