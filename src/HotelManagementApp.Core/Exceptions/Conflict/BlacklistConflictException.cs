using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.Conflict;

public class BlackListConflictException(string message) : ConflictException(message)
{
}
