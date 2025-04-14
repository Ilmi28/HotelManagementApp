using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.Conflict;

public class RoleConflictException(string message) : ConflictException(message)
{

}
