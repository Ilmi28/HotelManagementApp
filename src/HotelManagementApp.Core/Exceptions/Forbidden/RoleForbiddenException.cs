using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.Forbidden;

public class RoleForbiddenException(string message) : ForbiddenException(message)
{

}
