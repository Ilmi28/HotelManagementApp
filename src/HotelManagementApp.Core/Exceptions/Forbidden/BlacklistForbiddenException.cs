using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.Forbidden;

public class BlacklistForbiddenException(string message) : ForbiddenException(message)
{
    
}