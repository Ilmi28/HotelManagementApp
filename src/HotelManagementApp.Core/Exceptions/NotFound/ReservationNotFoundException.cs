using HotelManagementApp.Core.Exceptions.BaseExceptions;

namespace HotelManagementApp.Core.Exceptions.NotFound;

public class ReservationNotFoundException(string message) : NotFoundException(message)
{
    
}