using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.Conflict;

public class VIPConflictException(string message) : ConflictException(message)
{
}
