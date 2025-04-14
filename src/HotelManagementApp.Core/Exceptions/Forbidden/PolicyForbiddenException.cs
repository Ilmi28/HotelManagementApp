using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.Forbidden;

public class PolicyForbiddenException(string message) : ForbiddenException(message)
{

}
