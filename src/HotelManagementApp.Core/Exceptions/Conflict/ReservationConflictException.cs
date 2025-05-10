using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.Conflict;

public class ReservationConflictException(string message) : ConflictException(message)
{
    
}